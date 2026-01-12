namespace TaskTracker.Domain.DTOs.BoardMembers;

public class RejectInvitationRequest
{
    public int NotificationId { get; set; }

    public int InvitationId { get; set; }
}
