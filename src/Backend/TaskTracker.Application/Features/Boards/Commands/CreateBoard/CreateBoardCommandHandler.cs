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

    public async Task<int> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        using var uow = _uowFactory.Create();
        var board = new Board
        {
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        var newId = await uow.BoardRepository.AddAsync(board);

        uow.Commit();

        return newId;
    }
}
