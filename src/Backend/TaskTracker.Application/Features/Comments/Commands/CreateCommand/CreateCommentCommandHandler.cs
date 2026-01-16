using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Comments;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Comments.Commands.CreateCommand;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentDto>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public CreateCommentCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var comment = new Comment
        {
            Content = request.Content,
            CreatedAt = DateTime.UtcNow,
            TaskId = request.TaskId,
            CreatedBy = request.CreatedBy,
        };

        var id = await uow.CommentRepository.AddAsync(comment);

        if (id == 0)
        {
            return Result<CommentDto>.Failure("Failed to create comment.");
        }

        var user = await uow.UserRepository.GetAsync(request.CreatedBy);

        if (user is null)
        {
            return Result<CommentDto>.NotFound("User not found.");
        }

        comment.Creator = user;

        uow.Commit();

        comment.Id = id;

        return Result<CommentDto>.Success(comment.ToCommentDto());
    }
}
