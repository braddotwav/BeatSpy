using System;

namespace BeatSpy.Models;

public class Notification
{
    public virtual NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    
    public static readonly Notification Empty = new()
    {
        Type = NotificationType.INFO,
        Message = string.Empty
    };
}

public static class NotificationFactory
{
    public static Notification InfoNotification(string message) 
    {
        return new Notification()
        {
            Type = NotificationType.INFO,
            Message = message
        };
    }

    public static Notification ErrorNotification(string message)
    {
        return new Notification()
        {
            Type = NotificationType.ERROR,
            Message = message
        };
    }

    public static Notification ProgressNotification(string message, Action action)
    {
        return new ProgressNotification(action)
        {
            Message = message,
        };
    }
}

public enum NotificationType 
{
    INFO,
    ERROR,
    PROGRESS,
}