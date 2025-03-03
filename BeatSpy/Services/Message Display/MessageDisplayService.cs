using NLog;
using System;
using System.Diagnostics;

namespace BeatSpy.Services;

internal sealed class MessageDisplayService : IMessageDisplayService
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public event Action<string>? OnMessageReceived;

    public void ClearMessage()
    {
        OnMessageReceived?.Invoke(string.Empty);
    }

    public void DisplayErrorMessage(Exception ex)
    {
        OnMessageReceived?.Invoke(ex.Message);
        logger.Error(ex);
    }

    public void DisplayInfoMessage(string message, bool silent = false)
    {
        if (silent)
        {
            logger.Info(message);
            return;
        }

        OnMessageReceived?.Invoke(message);
        Debug.WriteLine(message);
    }
}