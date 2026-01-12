using MediatR;

namespace TaskTracker.Application.Features.BoardMembers.Commands.KickMember;

public class KickMemberCommand : IRequest<Result>
{
    public int BoardId { get; set; }

    public int UserId { get; set; }
}
