namespace BeatSpy.DataTypes.Constants;

public static class LogConstants
{
    #region AUTH CONSTANTS
    public const string AUTH_FILE_NOTFOUND = "Failed to find an existing authentication file. Log in to resolve.";
    public const string AUTH_FILE_CONTENTS_ERROR = "Failed to read authentication file. Log in to resolve.";
    public const string AUTH_REQUEST_FAILED = "Failed to retrieve a new request token.";
    #endregion

    #region SERVER CALLBACK CONSTANTS
    public const string SERVER_REQUEST_QUERY_ERROR = "Failed to retrieve query code. Please try again.";
    public const string SERVER_REQUEST_TOKEN_RESPONSE = "Failed to retrieve a token response. Please try again.";
    #endregion

    #region CLIENT CONSTANTS
    public const string CLIENT_CONNECTED = "Successfully connected to Spotify.";
    public const string CLIENT_DISCONNECTED = "Successfully disconnected from Spotify.";
    #endregion
}
