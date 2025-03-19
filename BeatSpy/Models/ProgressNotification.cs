using System;

namespace BeatSpy.Models;

public class ProgressNotification(Action cancelAction) : Notification
{
    public override NotificationType Type => NotificationType.PROGRESS;

    private readonly Action cancelAction = cancelAction;
    public void Cancel() => cancelAction?.Invoke();
}