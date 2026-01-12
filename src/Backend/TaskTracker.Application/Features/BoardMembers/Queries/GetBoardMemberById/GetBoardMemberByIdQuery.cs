using MediatR;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Application.Features.BoardMembers.Queries.GetBoardMemberById;

public class GetBoardMemberByIdQuery : IRequest<Result<MemberSummaryDto>>
{
    public int BoardId { get; set; }

    public int UserId { get; set; }
}
