using MediatR;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs;
using TaskTracker.Domain.DTOs.Comments;

namespace TaskTracker.Application.Features.Comments.Queries.GetComments;

public class GetCommentsQuery : IRequest<Result<PagedResponse<CommentDto>>>
{
    public int TaskId { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }
}
