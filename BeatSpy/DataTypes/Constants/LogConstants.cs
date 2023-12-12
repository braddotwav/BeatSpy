namespace BeatSpy.DataTypes.Constants;

public static class LogConstants
{
    #region AUTH CONSTANTS
    public const string AUTH_FILE_NOTFOUND = "Failed to find existing auth file, please log in to resolve.";
    public const string AUTH_FILE_CONTENTS_ERROR = "Failed to read the existing auth file, please log in to resolve";
    public const string AUTH_REQUEST_FAILED = "Failed to retrieve new request token.";
    #endregion

    #region SERVER CALLBACK CONSTANTS
    public const string SERVER_REQUEST_QUERY_ERROR = "Failed to retrieve the correct query code, please try again";
    #endregion

    #region CLIENT CONSTANTS
    public const string CLIENT_CONNECTED = "Successfully connected to spotify";
    public const string CLIENT_DISCONNECTED = "Successfully disconnected to spotify";
    #endregion
}
