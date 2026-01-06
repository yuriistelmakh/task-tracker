namespace TaskTracker.Domain.DTOs.Boards;

public class RejectInvitationRequest
{
    public int NotificationId { get; set; }

    public int InvitationId { get; set; }
}
