using MediatR;

namespace TaskTracker.Application.Features.BoardMembers.Commands.RejectInvitation;

public class RejectInvitationCommand : IRequest<bool>
{
    public int NotificationId { get; set; }

    public int InvitationId { get; set; }
}
