using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Boards;

public class SendInvitationsRequest
{
    public int InviterId { get; set; }

    public List<int> InviteeIds { get; set; } = [];

    public BoardRole Role { get; set; }
}
