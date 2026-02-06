using TaskTracker.Domain.DTOs.Comments;

namespace TaskTracker.WebApp.Models.Mapping;

public static class CommentMapping
{
    public static CommentModel ToCommentModel(this CommentDto dto) =>
        new()
        {
            Sender = dto.Sender.ToUserSummaryModel(),
            Content = dto.Content,
            CreatedAt = dto.CreatedAt
        };
}
