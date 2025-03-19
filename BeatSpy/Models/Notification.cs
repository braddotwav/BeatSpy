namespace BeatSpy.Models;

public class Notification
{
    public NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;

    public static readonly Notification Empty = new() 
    { 
        Type = NotificationType.INFO, 
        Message = string.Empty 
    };
}

public enum NotificationType 
{
    INFO,
    ERROR,
    PROGRESS,
}