using TaskTracker.Domain.Enums;

namespace TaskTracker.WebApp.Models.Notifications;

public class InvitationPayload
{
    public int Id { get; set; }

    public int InviteeId { get; set; }

    public int InviterId { get; set; }

    public int BoardId { get; set; }

    public BoardRole Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsAnswered { get; set; }
}
