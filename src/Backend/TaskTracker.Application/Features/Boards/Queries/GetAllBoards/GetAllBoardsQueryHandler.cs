using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;

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

        var boards = await uow.BoardRepository.GetAllWithOwnersAsync();

        var boardsDto = boards.Select(b => new BoardSummaryDto
        {
            Id = b.Id,
            Title = b.Title,
            IsArchived = b.IsArchived,
            Owner = new UserSummaryDto
            {
                Id = b.Creator.Id,
                Tag = b.Creator.Tag,
                DisplayName = b.Creator.DisplayName,
                AvatarUrl = b.Creator.AvatarUrl
            }
        });

        uow.Commit();

        return boardsDto;
    }
}
