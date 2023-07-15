using NLog;
using System;
using System.IO;
using System.Net;
using BeatSpy.Models;
using SpotifyAPI.Web;
using BeatSpy.Helpers;
using System.Text.Json;
using BeatSpy.ViewModels;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Constants;
using BeatSpy.DataTypes.Interfaces;

namespace BeatSpy.Auth;

internal class SpotifyPKCEAuth : ISpotifyService
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public SpotifyClient Client => spotifyClient!;
    
    private SpotifyClient? spotifyClient;

    private readonly string tokenFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BeatSpy/auth.json");
    private readonly MessageHandlerViewModel handler;
    private readonly Action onSpotifyConnected;

    public SpotifyPKCEAuth(MessageHandlerViewModel handler, Action onSpotifyConnected)
    {
        this.handler = handler;
        this.onSpotifyConnected = onSpotifyConnected;
    }

    public async Task Connect()
    {
        try
        {
            if (File.Exists(tokenFilePath))
            {
                //Auth data already exists
                logger.Info("Found existing auth token");
                string tokenContent = File.ReadAllText(tokenFilePath);
                var deserializeTokenJson = JsonSerializer.Deserialize<PKCETokenModel>(tokenContent);
                if (deserializeTokenJson != null)
                {
                    //Auth data has been loaded. Requesting a new token
                    logger.Info("Auth data has been loaded. Requesting a new token");
                    var newReponseCode = await TryGetPKCERefreshTokenResponse(deserializeTokenJson.RefreshToken);
                    if (newReponseCode != null)
                    {
                        deserializeTokenJson.RefreshToken = newReponseCode.RefreshToken;
                        string serializedJson = JsonSerializer.Serialize(deserializeTokenJson);
                        File.WriteAllText(tokenFilePath, serializedJson);
                        logger.Info($"New auth data has been saved to disk {tokenFilePath}");
                        spotifyClient = new SpotifyClient(newReponseCode.AccessToken);
                        onSpotifyConnected?.Invoke();
                        return;
                    }
                }
            }
            else
            {
                logger.Info("Failed to find existing auth file");
                handler.Message = "Could not find existing auth token. Please log in";

                var (verifier, challenge) = PKCEUtil.GenerateCodes(120);

                var loginRequest = new LoginRequest(
                  new Uri(AuthConstants.CALL_BACK),
                  Properties.Settings.Default.SpotClientId,
                  LoginRequest.ResponseType.Code
                )
                {
                    CodeChallengeMethod = "S256",
                    CodeChallenge = challenge,
                };

                var uri = loginRequest.ToUri();
                BrowsUtil.OpenUrl(uri.AbsoluteUri);

                await StartListenServer(verifier);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex);
        }
    }


    private async Task<PKCETokenResponse?> TryGetPKCERefreshTokenResponse(string refreshToken)
    {
        try
        {
            return await new OAuthClient().RequestToken(new PKCETokenRefreshRequest(Properties.Settings.Default.SpotClientId, refreshToken));
        }
        catch (Exception ex)
        {
            logger.Error(ex);
            handler.Message = "Error: Failed to get new request token";
            return null;
        }
    }

    private async Task StartListenServer(string verifier)
    {
        HttpListener listener = new();
        listener.Prefixes.Add(string.Concat(AuthConstants.CALL_BACK, "/"));
        try
        {
            listener.Start();
            logger.Info("Starting listen server for spotify callback");
            while(spotifyClient is null)
            {
                var context = listener.GetContext();
                if (context is not null)
                {
                    logger.Info("Received callback.. Checking if valid");
                    var content = context.Request.QueryString;
                    if (content.GetKey(0).Contains("code"))
                    {
                       await GetCallback(content["code"]!, verifier);
                    }
                    else
                    {
                        throw new Exception();
                    }
                    break;
                }
            }
        }
        catch(Exception ex)
        {
            logger.Error(ex);
            handler.Message = "Error: Failed to authorise. Please reload the application and try again.";
        }
        finally
        { 
            listener.Close(); 
        }
    }

    private async Task GetCallback(string code, string verifier)
    {
        var initialResponse = await new OAuthClient().RequestToken(
          new PKCETokenRequest(Properties.Settings.Default.SpotClientId, code, new Uri(AuthConstants.CALL_BACK), verifier)
        );

        if (initialResponse is not null)
        {
            logger.Info("Callback is valid");
            var serializedJson = JsonSerializer.Serialize(new PKCETokenModel() { RefreshToken = initialResponse.RefreshToken });
            File.WriteAllText(tokenFilePath, serializedJson);
            logger.Info("Auth is complete. Saving token data to disk");
            spotifyClient = new SpotifyClient(initialResponse.AccessToken);
            onSpotifyConnected?.Invoke();
        }
    }

    public bool IsConnected()
    {
        return spotifyClient is not null;
    }
}
