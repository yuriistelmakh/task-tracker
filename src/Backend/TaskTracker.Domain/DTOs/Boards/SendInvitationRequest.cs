using System.Collections.Generic;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Boards;

public class SendInvitationRequest
{
    public int InviterId { get; set; }

    public int InviteeId { get; set; }

    public BoardRole Role { get; set; }
}
