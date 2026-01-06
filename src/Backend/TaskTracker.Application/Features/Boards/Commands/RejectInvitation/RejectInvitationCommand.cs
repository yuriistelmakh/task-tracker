using MediatR;

namespace TaskTracker.Application.Features.Boards.Commands.RejectInvitation;

public class RejectInvitationCommand : IRequest<bool>
{
    public int NotificationId { get; set; }

    public int InvitationId { get; set; }
}
