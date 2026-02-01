using TaskTracker.WebApp.Models.Users;

namespace TaskTracker.WebApp.Models;

public class CommentModel
{
    public required UserSummaryModel Sender { get; set; }

    public required string Content { get; set; }

    public DateTime CreatedAt { get; set; }
}
