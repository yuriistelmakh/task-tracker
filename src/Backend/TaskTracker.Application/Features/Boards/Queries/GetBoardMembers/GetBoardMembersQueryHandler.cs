using MediatR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Boards.Queries.GetAllMembers;

public class GetBoardMembersQueryHandler : IRequestHandler<GetBoardMembersQuery, IEnumerable<MemberSummaryDto>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetBoardMembersQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<IEnumerable<MemberSummaryDto>> Handle(GetBoardMembersQuery request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var members = await uow.BoardRepository.GetMembersAsync(request.BoardId);

        uow.Commit();

        var dtos = members.Select(m => m.User.ToMemberSummaryDto(m.Role));

        return dtos;
    }
}
