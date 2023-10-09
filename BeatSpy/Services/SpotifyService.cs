using NLog;
using System;
using System.IO;
using System.Net;
using BeatSpy.Models;
using SpotifyAPI.Web;
using BeatSpy.Helpers;
using System.Text.Json;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Constants;

namespace BeatSpy.Services;

internal class SpotifyService : ISpotifyService
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public SpotifyClient Client => spotifyClient;
    private SpotifyClient? spotifyClient;

    private readonly string tokenFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/BeatSpy/auth.json";

    public event Action? OnConnected;
    public event Action? OnDisconnected;

    public async Task Connect()
    {
        try
        {
            if (File.Exists(tokenFilePath))
            {
                logger.Info(LogInfoConstants.AUTH_FILE_FOUND);
                string tokenContent = File.ReadAllText(tokenFilePath);
                SpotifyToken token = JsonSerializer.Deserialize<SpotifyToken>(tokenContent);
                if (token != null)
                {
                    logger.Info(LogInfoConstants.AUTH_FILE_LOADED);
                    PKCETokenResponse? newToken = await TryGetPKCERefreshTokenResponse(token.RefreshToken);
                    if (newToken != null)
                    {
                        token.RefreshToken = newToken.RefreshToken;
                        string serializedJson = JsonSerializer.Serialize(token);
                        File.WriteAllText(tokenFilePath, serializedJson);
                        logger.Info(LogInfoConstants.AUTH_FILE_UPDATED);
                        spotifyClient = new SpotifyClient(newToken.AccessToken);
                        OnConnected?.Invoke();
                        logger.Info(LogInfoConstants.SERVER_CONNECTED);
                        return;
                    }
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
        catch (Exception ex)
        {
            logger.Info(ex, LogInfoConstants.AUTH_FILE_NOTFOUND);
        }
    }

    public void Disconnect()
    {
        if(spotifyClient != null)
        {
            spotifyClient = null;
            File.Delete(tokenFilePath);
            OnDisconnected?.Invoke();
            logger.Info(LogInfoConstants.SERVER_DISCONNECTED);
        }
    }

    public bool IsConnected()
    {
        return spotifyClient != null;
    }

    public async Task Login()
    {
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

        BrowsUtil.OpenUrl(loginRequest.ToUri().AbsoluteUri);

        await StartListenServer(verifier);
    }

    private async Task StartListenServer(string verifier)
    {
        using HttpListener listener = new();
        listener.Prefixes.Add(string.Concat(AuthConstants.CALL_BACK, "/"));

        try
        {
            listener.Start();
            logger.Info(LogInfoConstants.SERVER_CALLBACK_INIT);

            while (spotifyClient == null)
            {
                HttpListenerContext context = listener.GetContext();
                if (context != null)
                {
                    logger.Info(LogInfoConstants.SERVER_CALLBACK_RECIVED);
                    if (context.Request.QueryString.Get("code") != null)
                    {
                        await GetCallback(context.Request.QueryString["code"]!, verifier);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, LogInfoConstants.SERVER_ERROR);
        }
    }

    private async Task GetCallback(string code, string verifier)
    {
        var initialResponse = await new OAuthClient().RequestToken(
          new PKCETokenRequest(Properties.Settings.Default.SpotClientId, code, new Uri(AuthConstants.CALL_BACK), verifier)
        );

        if (initialResponse != null)
        {
            var serializedJson = JsonSerializer.Serialize(new SpotifyToken() { RefreshToken = initialResponse.RefreshToken });
            File.WriteAllText(tokenFilePath, serializedJson);
            logger.Info(LogInfoConstants.AUTH_FILE_UPDATED);
            spotifyClient = new SpotifyClient(initialResponse.AccessToken);
            OnConnected?.Invoke();
        }
    }

    private async Task<PKCETokenResponse?> TryGetPKCERefreshTokenResponse(string refreshToken)
    {
        try
        {
            PKCETokenResponse reponse = await new OAuthClient().RequestToken(new PKCETokenRefreshRequest(Properties.Settings.Default.SpotClientId, refreshToken)) ?? throw new NullReferenceException();
            logger.Info(LogInfoConstants.AUTH_REQUEST_SUCCESS);
            return reponse;
        }
        catch (Exception ex)
        {
            logger.Error(ex, LogInfoConstants.AUTH_REQUEST_FAILED);
            return null;
        }
    }
}