using TaskTracker.Domain.DTOs.Notifications;
using TaskTracker.WebApp.Models.Notifications;

namespace TaskTracker.WebApp.Models.Mapping;

public static class NotificationMapping
{
    public static NotificationModel ToNotificationModel(this NotificationDto dto) =>
        new()
        {
            Id = dto.Id,
            Message = dto.Message,
            Payload = dto.Payload,
            Title = dto.Title,
            IsRead = dto.IsRead,
            CreatedAt = dto.CreatedAt,
            Type = dto.Type,
            UserId = dto.UserId,
        };
}
