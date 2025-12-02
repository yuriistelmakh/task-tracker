using MediatR;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Boards.Commands.CreateBoard;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, int>
{
    private readonly IBoardRepository _boardRepository;

    public CreateBoardCommandHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<int> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var board = new Board
        {
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        var newId = await _boardRepository.AddAsync(board);

        return newId;
    }
}
