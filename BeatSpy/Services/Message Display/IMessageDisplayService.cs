using System;

namespace BeatSpy.Services;

public interface IMessageDisplayService
{
    public string Message { get; }
    public bool IsMessageEmpty { get; }
    public event Action OnMessageUpdated;
    public void DisplayInfoMessage(string message, bool silent = false);
    public void DisplayErrorMessage(Exception ex);
    public void ClearMessage();
}