using NLog;
using System;

namespace BeatSpy.Services;

internal sealed class MessageDisplayService : IMessageDisplayService
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private string message = string.Empty;

    public string Message
    {
        get { return message; }
        set
        {
            message = value;
            OnMessageUpdated?.Invoke();
        }
    }

    public bool IsMessageEmpty => string.IsNullOrEmpty(message);

    public event Action? OnMessageUpdated;

    /// <summary>
    /// Clears the message to a empty string
    /// </summary>
    public void ClearMessage()
    {
        Message = string.Empty;
    }

    /// <summary>
    /// Displays an exception message and logs the exception.
    /// </summary>
    /// <param name="ex"></param>
    public void DisplayErrorMessage(Exception ex)
    {
        Message = ex.Message;
        logger.Error(ex);
    }

    /// <summary>
    /// Displays an info message to the UI with the option of being silent.
    /// </summary>
    /// <param name="message">Message to log</param>
    /// <param name="silent">Should the message be shown to the user</param>
    public void DisplayInfoMessage(string message, bool silent)
    {
        Message = silent ? string.Empty : message;
        logger.Info(message);
    }
}