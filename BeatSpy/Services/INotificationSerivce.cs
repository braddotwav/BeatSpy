using System;
using BeatSpy.Models;

namespace BeatSpy.Services;

interface INotificationSerivce
{
    public event Action<Notification> OnNotificationRecived;
    public void ShowNotification(Notification notification);
    public void ClearNotification();
}