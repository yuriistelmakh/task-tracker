using TaskTracker.Domain.DTOs.Comments;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Mapping;

public static class CommentMapping
{
    public static CommentDto ToCommentDto(this Comment comment) =>
        new()
        {
            Sender = comment.Creator.ToUserSummaryDto(),
            Content = comment.Content,
            CreatedAt = comment.CreatedAt
        };
}
