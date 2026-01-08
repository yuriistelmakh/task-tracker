using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.BoardMembers;

public class SendInvitationRequest
{
    public int InviterId { get; set; }

    public int InviteeId { get; set; }

    public BoardRole Role { get; set; }
}
