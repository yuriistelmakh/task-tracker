using MediatR;
using System.Collections.Generic;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Boards.Commands.SendInvitations;

public class SendInvitationsCommand : IRequest<Result>
{
    public int InviterId { get; set; }

    public List<int> InviteeIds { get; set; } = [];

    public int BoardId { get; set; }

    public BoardRole Role { get; set; }
}
