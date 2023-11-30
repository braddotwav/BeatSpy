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

    private readonly string serviceCallBack = "http://localhost:5543/callback";
    private readonly string tokenFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/BeatSpy/AppAuth.json";

    public event Action? OnConnected;
    public event Action? OnDisconnected;
    public event Action<string>? OnServiceError;

    /// <summary>
    /// Starts the connect process of the spotify client
    /// </summary>
    /// <returns></returns>
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
            switch(ex)
            {
                case FileNotFoundException:
                    OnServiceError?.Invoke(LogInfoConstants.AUTH_FILE_NOTFOUND);
                    break;
                default:
                    logger.Info(ex, ex.Message);
                    break;
            }
        }
    }

    /// <summary>
    /// Disconnect from spotify
    /// </summary>
    public void Disconnect()
    {
        if(IsConnected())
        {
            spotifyClient = null;
            File.Delete(tokenFilePath);
            OnDisconnected?.Invoke();
            logger.Info(LogInfoConstants.SERVER_DISCONNECTED);
        }
    }

    /// <summary>
    /// Check if the spotify client is null
    /// </summary>
    /// <returns></returns>
    public bool IsConnected()
    {
        return spotifyClient != null;
    }

    /// <summary>
    /// Starts the login process
    /// </summary>
    /// <returns></returns>
    public async Task Login()
    {
        var (verifier, challenge) = PKCEUtil.GenerateCodes(120);

        var loginRequest = new LoginRequest(
          new Uri(serviceCallBack),
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

    /// <summary>
    /// Starts a listen server for the OAuth process
    /// </summary>
    /// <param name="verifier">PKCE verifer</param>
    /// <returns></returns>
    private async Task StartListenServer(string verifier)
    {
        using HttpListener listener = new();
        listener.Prefixes.Add(string.Concat(serviceCallBack, "/"));

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
            InvokeServiceError(ex, LogInfoConstants.SERVER_ERROR);
        }
    }

    /// <summary>
    /// Manages the callback response from the listen server
    /// </summary>
    /// <param name="code">The recived code from the listen server</param>
    /// <param name="verifier">PKCE verifer that was provided to the listen server</param>
    /// <returns></returns>
    private async Task GetCallback(string code, string verifier)
    {
        var initialResponse = await new OAuthClient().RequestToken(
          new PKCETokenRequest(Properties.Settings.Default.SpotClientId, code, new Uri(serviceCallBack), verifier)
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

    /// <summary>
    /// Attempts to retrieve a PKCE token response using a provided refresh token.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
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
            InvokeServiceError(ex, LogInfoConstants.AUTH_REQUEST_FAILED);
            return null;
        }
    }

    /// <summary>
    /// Invokes a service error sending it to the 
    /// message handler and the logger
    /// </summary>
    /// <param name="ex">The exception</param>
    /// <param name="message">Service message</param>
    private void InvokeServiceError(Exception ex, string message)
    {
        OnServiceError?.Invoke(message);
        logger.Error(ex, message);
    }
}