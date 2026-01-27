using TaskTracker.Domain.DTOs.Comments;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Mapping;

public static class CommentMapping
{
    public static CommentDto ToCommentDto(this Comment comment) =>
        new()
        {
            SenderName = comment.Creator.DisplayName,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt
        };
}
