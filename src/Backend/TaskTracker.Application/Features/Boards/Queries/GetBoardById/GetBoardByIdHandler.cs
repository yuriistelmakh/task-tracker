using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.DTOs;

namespace TaskTracker.Application.Features.Boards.Queries.GetBoardById;

public class GetBoardByIdHandler : IRequestHandler<GetBoardByIdQuery, BoardDetailsDto?>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetBoardByIdHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<BoardDetailsDto?> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();
        var board = await uow.BoardRepository.GetAsync(request.Id);

        if (board is null)
        {
            return null;
        }

        return new BoardDetailsDto
        {
            Id = board.Id,
            Title = board.Title,
            Description = board.Description,
            CreatedAt = board.CreatedAt,
            Owner = null //TODO
        };
    }
}
