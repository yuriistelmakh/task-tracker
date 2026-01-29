using System;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Domain.DTOs.Attachments;

public class AttachmentDto
{
    public int Id { get; set; }

    public UserSummaryDto CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public required string FileUrl { get; set; }

    public required string Name { get; set; }

    public double SizeKB { get; set; }
}