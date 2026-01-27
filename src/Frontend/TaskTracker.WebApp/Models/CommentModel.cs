namespace TaskTracker.WebApp.Models;

public class CommentModel
{
    public required string SenderName { get; set; }

    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; }
}
