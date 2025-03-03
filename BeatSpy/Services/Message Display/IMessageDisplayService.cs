using System;

namespace BeatSpy.Services;

public interface IMessageDisplayService
{
    public event Action<string> OnMessageReceived;

    /// <summary>
    /// Displays a message with the option of being silent.
    /// </summary>
    /// <param name="message">Message to log</param>
    /// <param name="silent">Should the message be shown to the user</param>
    public void DisplayInfoMessage(string message, bool silent = false);

    /// <summary>
    /// Displays and logs an exception message
    /// </summary>
    /// <param name="ex"></param>
    public void DisplayErrorMessage(Exception ex);

    /// <summary>
    /// Clears the message to a empty string
    /// </summary>
    public void ClearMessage();
}