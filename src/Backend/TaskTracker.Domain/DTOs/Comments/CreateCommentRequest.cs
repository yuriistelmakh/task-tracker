namespace TaskTracker.Domain.DTOs.Comments;

public class CreateCommentRequest
{
    public int CreatedBy { get; set; }

    public int TaskId { get; set; }

    public required string Content { get; set; }
}
