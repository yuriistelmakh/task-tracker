using System;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities;

[Table("Invitations")]
public class Invitation
{
    public int Id { get; set; }

    public int InviterId { get; set; }

    public int InviteeId { get; set; }

    public int BoardId { get; set; }

    public BoardRole Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsAnswered { get; set; }
}
