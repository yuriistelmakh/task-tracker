using System;

namespace TaskTracker.Domain.DTOs.Comments;

public class CommentDto
{
    public required string SenderName { get; set; }

    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; }
}
