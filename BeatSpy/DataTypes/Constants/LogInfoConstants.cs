namespace BeatSpy.DataTypes.Constants;

internal static class LogInfoConstants
{
    #region AUTH CONSTANTS
    public const string AUTH_FILE_FOUND = "Successfully found existing auth token.";
    public const string AUTH_FILE_NOTFOUND = "Failed to find existing auth token, please log in to resolve.";
    public const string AUTH_FILE_LOADED = "Successfully loaded previous auth data.";
    public const string AUTH_FILE_UPDATED = "Successfully saved new auth data to disk.";
    public const string AUTH_REQUEST_SUCCESS = "Successfully retrieved new request token.";
    public const string AUTH_REQUEST_FAILED = "Failed to retrieve new request token.";
    #endregion

    #region SERVER CALLBACK CONSTANTS
    public const string SERVER_CALLBACK_INIT = "Successfully started listen server.";
    public const string SERVER_CALLBACK_RECIVED = "Successfully received callback.";
    public const string SERVER_ERROR = "Failed to authorise, please try again";
    public const string SERVER_CONNECTED = "Successfully connected to spotify";
    public const string SERVER_DISCONNECTED = "Successfully disconnected to spotify";
    #endregion

    #region SEARCH CONSTANTS
    public const string LOG_SEARCH_OUTOFRANGE = "Failed to fetch a result for";
    public const string LOG_SEARCH_FAILED = "Failed to fetch search request.";
    public const string LOG_SEARCH_SUCCESS = "Successfully fetched a result for";
    #endregion
}
