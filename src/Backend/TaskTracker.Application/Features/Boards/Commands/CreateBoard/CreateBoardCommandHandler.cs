using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Boards.Commands.CreateBoard;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, int>
{
    private readonly IUnitOfWorkFactory _uowFactory;

    public CreateBoardCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _uowFactory = unitOfWorkFactory;
    }

    public async Task<int> Handle(CreateBoardCommand command, CancellationToken cancellationToken)
    {
        using var uow = _uowFactory.Create();

        var board = new Board
        {
            Title = command.Title,
            Description = command.Description,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = command.CreatedBy
        };

        var newId = await uow.BoardRepository.AddAsync(board);

        uow.Commit();

        return newId;
    }
}
