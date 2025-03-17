using System;
using BeatSpy.Models;

namespace BeatSpy.Services;

interface INotificationSerivce
{
    public event Action<Notification> OnNotificationRecived;
    public void ShowNotification(NotificationType type, string message);
    public void ClearNotification();
}