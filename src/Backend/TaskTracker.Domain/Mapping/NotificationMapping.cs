using TaskTracker.Domain.DTOs.Notifications;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Mapping;

public static class NotificationMapping
{
    public static NotificationDto ToNotificationDto(this Notification notification) =>
        new()
        {
            Id = notification.Id,
            Message = notification.Message,
            Payload = notification.Payload,
            CreatedAt = notification.CreatedAt,
            IsRead = notification.IsRead,
            Title = notification.Title,
            Type = notification.Type,
            UserId = notification.UserId
        };
}
