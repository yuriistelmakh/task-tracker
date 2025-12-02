using MediatR;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Boards.Queries.GetBoardById;

public class GetBoardByIdHandler : IRequestHandler<GetBoardByIdQuery, BoardDto?>
{
    private readonly IBoardRepository _repository;

    public GetBoardByIdHandler(IBoardRepository repository)
    {
        _repository = repository;
    }

    public async Task<BoardDto?> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        var board = await _repository.GetAsync(request.Id);

        if (board is null)
        {
            return null;
        }

        return new BoardDto
        {
            Id = board.Id,
            Title = board.Title,
            Description = board.Description,
            CreatedAt = board.CreatedAt
        };
    }
}
