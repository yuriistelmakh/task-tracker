using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Notifications;

public class NotificationDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public NotificationType Type { get; set; }

    public required string Payload { get; set; }

    public required string Title { get; set; }

    public required string Message { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }
}
