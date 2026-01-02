using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Boards.Commands.CreateBoard;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, int>
{
    private readonly IUnitOfWorkFactory _uowFactory;

    public CreateBoardCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _uowFactory = unitOfWorkFactory;
    }

    public async Task<int> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        using var uow = _uowFactory.Create();

        var board = new Board
        {
            Title = request.Title,
            IsArchived = false,
            DisplayColor = request.DisplayColor,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.CreatedBy,
        };

        var newId = await uow.BoardRepository.AddAsync(board);

        var boardMember = new BoardMember
        {
            BoardId = newId,
            UserId = board.CreatedBy,
            JoinedAt = DateTime.UtcNow,
            Role = BoardRole.Owner,
        };

        await uow.BoardRepository.AddMemberAsync(boardMember);

        uow.Commit();

        return newId;
    }
}
