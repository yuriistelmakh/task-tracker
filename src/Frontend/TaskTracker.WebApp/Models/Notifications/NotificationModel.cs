using TaskTracker.Domain.Enums;

namespace TaskTracker.WebApp.Models.Notifications;

public class NotificationModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public NotificationType Type { get; set; }

    public required string Payload { get; set; }

    public required string Title { get; set; }

    public required string Message { get; set; }

    public string? MessageIcon { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }
}
