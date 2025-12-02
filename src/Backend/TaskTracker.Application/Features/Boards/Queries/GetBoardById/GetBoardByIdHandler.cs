using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Boards.Queries.GetBoardById;

public class GetBoardByIdHandler : IRequestHandler<GetBoardByIdQuery, BoardDto?>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetBoardByIdHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<BoardDto?> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        using (var uow = _unitOfWorkFactory.Create())
        {
            var board = await uow.Boards.GetAsync(request.Id);

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
}
