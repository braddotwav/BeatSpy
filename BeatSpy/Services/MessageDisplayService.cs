using NLog;
using System;
using BeatSpy.DataTypes.Enums;

namespace BeatSpy.Services;

internal class MessageDisplayService : IMessageDisplayService
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
    /// Displays and logs error that has been thrown
    /// </summary>
    /// <param name="ex">Exception to be shown</param>
    public void DisplayErrorMessage(Exception ex)
    {
        Message = ex.Message;
        logger.Error(ex);
    }

    /// <summary>
    /// Displays and logs message based on the specified type
    /// </summary>
    /// <param name="messageType">Message type</param>
    /// <param name="message">Message to be shown</param>
    public void DisplayInfoMessage(MessageType messageType, string message)
    {
        if (messageType == MessageType.Normal)
            Message = message;

        logger.Info(message);
    }
}