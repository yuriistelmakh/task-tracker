using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Domain.DTOs.Comments;

public class CommentDto
{
    public required UserSummaryDto Sender { get; set; }

    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; }
}
