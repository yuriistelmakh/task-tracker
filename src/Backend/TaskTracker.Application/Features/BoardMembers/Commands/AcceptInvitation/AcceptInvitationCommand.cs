using MediatR;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.BoardMembers.Commands.AcceptInvitation;

public class AcceptInvitationCommand : IRequest<Result>
{
    public int NotificationId { get; set; }

    public int InvitationId { get; set; }

    public int InviteeId { get; set; }

    public int BoardId { get; set; }

    public BoardRole Role { get; set; }
}
