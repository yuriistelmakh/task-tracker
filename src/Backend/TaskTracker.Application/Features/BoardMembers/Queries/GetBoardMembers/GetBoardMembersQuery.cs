using MediatR;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Application.Features.BoardMembers.Queries.GetBoardMembers;

public class GetBoardMembersQuery : IRequest<IEnumerable<MemberSummaryDto>>
{
    public int BoardId { get; set; }

    public int? Page { get; set; }

    public int? PageSize { get; set; }
}
