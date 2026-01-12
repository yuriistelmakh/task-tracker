using System;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities;

[Table("Notifications")]
public class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public required string Payload { get; set; }

    public NotificationType Type { get; set; }

    public required string Title { get; set; }

    public required string Message { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }
}
