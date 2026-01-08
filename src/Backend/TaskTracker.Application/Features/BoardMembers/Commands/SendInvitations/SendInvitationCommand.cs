using MediatR;
using System.Collections.Generic;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.BoardMembers.Commands.SendInvitations;

public class SendInvitationCommand : IRequest<Result>
{
    public int InviterId { get; set; }

    public int InviteeId { get; set; }

    public int BoardId { get; set; }

    public BoardRole Role { get; set; }
}
