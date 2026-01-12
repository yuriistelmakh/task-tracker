using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Boards;

public class AcceptInvitationRequest
{
    public int NotificationId { get; set; }

    public int InvitationId { get; set; }

    public int InviteeId { get; set; }

    public BoardRole Role { get; set; }
}
