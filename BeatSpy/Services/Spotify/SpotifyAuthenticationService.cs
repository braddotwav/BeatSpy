using System;
using System.IO;
using System.Net;
using BeatSpy.Models;
using SpotifyAPI.Web;
using BeatSpy.Helpers;
using System.Net.Http;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Constants;

namespace BeatSpy.Services;

internal sealed class SpotifyAuthenticationService : ISpotifyAuthenticationService
{
    private const string CLIENT_ID = "4b675b489cf74ff2a4a2d4483cf1dbe1";
    private const string CALL_BACK = "http://localhost:5543/callback";

    private PKCEAuthenticator? authenticator;

    public async Task LoginAsync()
    {
        var (verifier, challenge) = PKCEUtil.GenerateCodes(120);

        LoginRequest loginRequest = new(new Uri(CALL_BACK), CLIENT_ID, LoginRequest.ResponseType.Code)
        {
            CodeChallengeMethod = "S256",
            CodeChallenge = challenge,
        };

        BrowserHelper.OpenURLInBrowser(loginRequest.ToUri().AbsoluteUri);

        await StartServiceCallbackListener(verifier);
    }

    public async Task<SpotifyClient> ConnectAsync()
    {
        if (AuthenticationHelper.TryGetAuthFile(out string contents))
        {
            SpotifyToken userToken = AuthenticationHelper.DeserializeTokenContent(contents) ?? throw new InvalidDataException(LogConstants.AUTH_FILE_CONTENTS_ERROR);
            PKCETokenResponse tokenReponse = await AuthenticationHelper.PKCETokenRefreshResponseAsync(CLIENT_ID, userToken.RefreshToken!);

            //Update the users refresh token and serialize it to the authentication file
            userToken.RefreshToken = tokenReponse.RefreshToken;
            AuthenticationHelper.SerializeTokenContent(userToken);

            //Set up the authenticator and config to re-grab a token when it expires
            authenticator = new PKCEAuthenticator(CLIENT_ID, tokenReponse);
            authenticator.TokenRefreshed += AuthenticatorTokenRefreshed;
            SpotifyClientConfig config = SpotifyClientConfig.CreateDefault().WithAuthenticator(authenticator);

            //Return a spotify client
            return new SpotifyClient(config);
        }
        else
        {
            throw new FileNotFoundException(LogConstants.AUTH_FILE_NOTFOUND);
        }
    }

    public void LogOut()
    {
        File.Delete(DataHelper.GetFullFilePath(FileConstants.AUTH_FILE));
    }

    private void AuthenticatorTokenRefreshed(object? sender, PKCETokenResponse e)
    {
        UpdateTokenRefresh(e.RefreshToken);
    }

    private void UpdateTokenRefresh(string refreshToken)
    {
        AuthenticationHelper.SerializeTokenContent(new SpotifyToken()
        {
            RefreshToken = refreshToken,
        });
    }

    private async Task StartServiceCallbackListener(string verifier)
    {
        using HttpListener serviceListener = new();
        serviceListener.Prefixes.Add(string.Concat(CALL_BACK, "/"));

        try
        {
            serviceListener.Start();
            while (serviceListener.IsListening)
            {
                HttpListenerContext context = await serviceListener.GetContextAsync();
                if (context != null)
                {
                    string requestQueryCode = context.Request.QueryString.Get("code") ?? throw new HttpRequestException(LogConstants.SERVER_REQUEST_QUERY_ERROR);
                    await GetCallBack(requestQueryCode, verifier);
                    serviceListener.Stop();
                    break;
                }
            }
        }
        finally
        {
            serviceListener.Close();
        }
    }

    private async Task GetCallBack(string code, string verifier)
    {
        PKCETokenResponse response = await AuthenticationHelper.PKCETokenRequestResponseAsync(CLIENT_ID, CALL_BACK, code, verifier) ?? throw new InvalidOperationException(LogConstants.SERVER_REQUEST_TOKEN_RESPONSE);
        UpdateTokenRefresh(response.RefreshToken);
    }

    public void Dispose()
    {
        authenticator!.TokenRefreshed -= AuthenticatorTokenRefreshed;
    }
}