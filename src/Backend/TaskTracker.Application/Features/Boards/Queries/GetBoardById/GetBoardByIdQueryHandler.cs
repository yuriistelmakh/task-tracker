using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Boards.Queries.GetBoardById;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, BoardDetailsDto?>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetBoardByIdQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<BoardDetailsDto?> Handle(GetBoardByIdQuery request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var board = await uow.BoardRepository.GetDetailsAsync(request.Id);

        var members = await uow.MemberRepository.GetAllAsync(request.Id);

        var memberStatistics = new BoardMemberStatisticsDto
        {
            TotalMembers = members.Count(),
            OwnersCount = members.Count(m => m.Role == BoardRole.Owner),
            AdministratorsCount = members.Count(m => m.Role == BoardRole.Admin),
            MembersCount = members.Count(m => m.Role == BoardRole.Member),
            VisitorsCount = members.Count(m => m.Role == BoardRole.Visitor)
        };

        uow.Commit();

        return board?.ToBoardDetailsDto(memberStatistics);
    }
}
