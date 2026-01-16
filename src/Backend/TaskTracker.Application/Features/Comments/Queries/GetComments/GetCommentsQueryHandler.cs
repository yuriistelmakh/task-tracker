using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs;
using TaskTracker.Domain.DTOs.Comments;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Comments.Queries.GetComments;

public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, Result<PagedResponse<CommentDto>>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetCommentsQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result<PagedResponse<CommentDto>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var comments = await uow.CommentRepository.GetAllAsync(request.TaskId, request.Page, request.PageSize);

        uow.Commit();

        var response = new PagedResponse<CommentDto>()
        {
            Items = comments.Select(c => c.ToCommentDto())
        };

        return Result<PagedResponse<CommentDto>>.Success(response);
    }
}
