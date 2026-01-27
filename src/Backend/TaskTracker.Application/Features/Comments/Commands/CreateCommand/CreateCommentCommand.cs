using MediatR;
using TaskTracker.Domain.DTOs.Comments;

namespace TaskTracker.Application.Features.Comments.Commands.CreateCommand;

public class CreateCommentCommand : IRequest<Result<CommentDto>>
{
    public int TaskId { get; set; }

    public int CreatedBy { get; set; }

    public required string Content { get; set; }
}
