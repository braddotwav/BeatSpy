using System;
using BeatSpy.Models;

namespace BeatSpy.Services;

internal class NotificationService : INotificationSerivce
{
    public event Action<Notification>? OnNotificationRecived;

    public void ClearNotification()
    {
        OnNotificationRecived?.Invoke(Notification.Empty);
    }

    public void ShowNotification(NotificationType type, string message)
    {
        OnNotificationRecived?.Invoke(new Notification()
        {
            Type = type,
            Message = message
        });
    }
}