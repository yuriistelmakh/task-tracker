using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Boards.Queries.GetAllBoards;

public class GetAllBoardsQueryHandler : IRequestHandler<GetAllBoardsQuery, PagedResponse<BoardSummaryDto>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetAllBoardsQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<PagedResponse<BoardSummaryDto>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var boards = await uow.BoardRepository.GetAllWithDetailsAsync(request.UserId, request.Page, request.PageSize);
        var totalBoardsCount = await uow.BoardRepository.GetCountAsync(request.UserId);

        var boardsDto = boards.Select(b => b.ToBoardSummaryDto());
        
        uow.Commit();

        var response = new PagedResponse<BoardSummaryDto>
        {
            Items = boardsDto,
            TotalCount = totalBoardsCount
        };

        return response;
    }
}
