using BeatSpy.DataTypes.Enums;
using System;

namespace BeatSpy.Services;

public interface IMessageDisplayService
{
    public string Message { get; }
    public bool IsMessageEmpty { get; }
    public event Action OnMessageUpdated;
    public void DisplayInfoMessage(MessageType messageType, string message);
    public void DisplayErrorMessage(Exception ex);
    public void ClearMessage();
}