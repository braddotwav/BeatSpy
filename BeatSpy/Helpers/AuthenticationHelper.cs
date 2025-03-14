using System;
using System.IO;
using BeatSpy.Models;
using SpotifyAPI.Web;
using System.Text.Json;
using System.Threading.Tasks;
using BeatSpy.DataTypes.Constants;

namespace BeatSpy.Helpers;

public static class AuthenticationHelper
{
    /// <summary>
    /// Retrieves and deserializes the Spotify authentication token from local storage.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="InvalidDataException"></exception>
    public static UserToken GetToken()
    {
        string authPath = DataHelper.GetFullFilePath(FileConstants.AUTH_FILE);

        if (!File.Exists(authPath))
            throw new FileNotFoundException(LogConstants.AUTH_FILE_NOTFOUND);

        string content = File.ReadAllText(authPath);

        var token = JsonSerializer.Deserialize<UserToken>(content);
        return token ?? throw new InvalidDataException(LogConstants.AUTH_FILE_CONTENTS_ERROR);
    }

    /// <summary>
    /// Serializes the provided authentication token and writes to data path
    /// </summary>
    /// <param name="authToken">Authentication object</param>
    public static void SerializeAndStoreToken(string token)
    {
        string serializedToken = JsonSerializer.Serialize(new UserToken() 
        { 
            Token = token
        });

        File.WriteAllText(DataHelper.GetFullFilePath(FileConstants.AUTH_FILE), serializedToken);
    }

    /// <summary>
    /// Initiates an asynchronous PKCE token request using provided client ID, callback URL, code, and verifier.
    /// </summary>
    /// <param name="clientID">Spotify client ID</param>
    /// <param name="callBack">Callback Url</param>
    /// <param name="code">The code query from the listener context</param>
    /// <param name="verifier">Verifier code</param>
    /// <returns></returns>
    public static async Task<PKCETokenResponse> PKCETokenRequestResponseAsync(string clientid, string callBack, string code, string verifier)
    {
        PKCETokenResponse reponse = await new OAuthClient().RequestToken(new PKCETokenRequest(clientid, code, new Uri(callBack), verifier));
        return reponse;
    }

    /// <summary>
    /// Initiates an asynchronous PKCE token refresh request using provided client ID and the current refresh token
    /// </summary>
    /// <param name="clientID">Spotify client ID</param>
    /// <param name="refreshToken">Current refresh token</param>
    /// <returns></returns>
    public static async Task<PKCETokenResponse> PKCETokenRefreshResponseAsync(string clientid, string refreshToken)
    {
        PKCETokenResponse response = await new OAuthClient().RequestToken(new PKCETokenRefreshRequest(clientid, refreshToken));
        return response;
    }
}