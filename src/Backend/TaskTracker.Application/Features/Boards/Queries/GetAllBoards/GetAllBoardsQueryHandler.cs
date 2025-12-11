using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Boards.Queries.GetAllBoards;

public class GetAllBoardsQueryHandler : IRequestHandler<GetAllBoardsQuery, IEnumerable<BoardSummaryDto>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetAllBoardsQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<IEnumerable<BoardSummaryDto>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var boards = await uow.BoardRepository.GetAllWithDetailsAsync(request.UserId);

        var boardsDto = boards.Select(b => b.ToBoardSummaryDto());

        uow.Commit();

        return boardsDto;
    }
}
