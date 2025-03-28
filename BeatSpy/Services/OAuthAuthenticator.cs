using System;
using System.IO;
using System.Net;
using System.Text;
using BeatSpy.Models;
using SpotifyAPI.Web;
using BeatSpy.Helpers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Resources;
using System.Windows;

namespace BeatSpy.Services;

internal class OAuthAuthenticator : IDisposable
{
    private readonly string clientId;
    private readonly Uri redirectUri;
    private readonly HttpListener httpListener;

    private PKCEAuthenticator? authenticator;

    public OAuthAuthenticator(string clientId, Uri redirectUri)
    {
        this.clientId = clientId;
        this.redirectUri = redirectUri;

        httpListener = new HttpListener();
        httpListener.Prefixes.Add(string.Concat(redirectUri, "/"));
    }

    public async Task Authenticate()
    {
        var (verifier, challenge) = PKCEUtil.GenerateCodes(120);

        LoginRequest loginRequest = new(redirectUri, clientId, LoginRequest.ResponseType.Code)
        {
            CodeChallengeMethod = "S256",
            CodeChallenge = challenge
        };

        BrowserHelper.OpenURLInBrowser(loginRequest.ToUri().AbsoluteUri);
        await StartCallbackListener(verifier);
    }

    private async Task StartCallbackListener(string verifier)
    {
        httpListener.Start();
        HttpListenerContext context = await httpListener.GetContextAsync();

        string? code = context.Request.QueryString["code"];

        await SendReponseAsync(context.Response);

        if (code == null)
            throw new InvalidOperationException("Failed to retrieve query code. Please try again.");

        await GetCallBack(code, verifier);
    }

    private async Task SendReponseAsync(HttpListenerResponse reponse)
    {
        string reponseContent = GetReponseHTML();
        byte[] buffer = Encoding.UTF8.GetBytes(reponseContent);

        reponse.ContentLength64 = buffer.Length;
        await reponse.OutputStream.WriteAsync(buffer);
    }

    private string GetReponseHTML()
    {
        Uri uri = new("/BeatSpy;component/Resources/Authentication/authentication.html", UriKind.Relative);
        StreamResourceInfo info = Application.GetResourceStream(uri);
        using StreamReader reader = new(info.Stream);
        return reader.ReadToEnd();
    }

    private async Task GetCallBack(string code, string verifier)
    {
        PKCETokenResponse response = await new OAuthClient().RequestToken(new PKCETokenRequest(clientId, code, redirectUri, verifier));
        SerializeAndStoreToken(response.RefreshToken);
    }

    public async Task<SpotifyClient> ConnectAsync()
    {
        UserToken userToken = GetUserToken();
        PKCETokenResponse response = await new OAuthClient().RequestToken(new PKCETokenRefreshRequest(clientId, userToken.Token));

        SerializeAndStoreToken(response.RefreshToken);

        authenticator = new PKCEAuthenticator(clientId, response);
        authenticator.TokenRefreshed += Authenticator_TokenRefreshed;

        SpotifyClientConfig config = SpotifyClientConfig.CreateDefault().WithAuthenticator(authenticator);

        return new SpotifyClient(config);
    }

    private void Authenticator_TokenRefreshed(object? sender, PKCETokenResponse e)
    {
        SerializeAndStoreToken(e.RefreshToken);
    }

    public void Disconnect()
    {
        File.Delete(GetAuthenticationFilePath());
    }

    private static UserToken GetUserToken()
    {
        if (!File.Exists(GetAuthenticationFilePath()))
            throw new FileNotFoundException("Failed to find an existing authentication file. Log in to resolve.");

        string content = File.ReadAllText(GetAuthenticationFilePath());

        var token = JsonSerializer.Deserialize<UserToken>(content);
        return token ?? throw new InvalidDataException("Failed to deserialize the authentication file. Log in to resolve.");
    }

    private static void SerializeAndStoreToken(string token)
    {
        string serializedToken = JsonSerializer.Serialize(new UserToken()
        {
            Token = token
        });

        File.WriteAllText(GetAuthenticationFilePath(), serializedToken);
    }

    private static string GetAuthenticationFilePath()
    {
        return DataHelper.GetFullFilePath("Authentication.json");
    }

    public void Dispose()
    {
        httpListener.Stop();

        if (authenticator != null)
            authenticator.TokenRefreshed -= Authenticator_TokenRefreshed;
    }
}