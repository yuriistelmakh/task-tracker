using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Domain.Entities;

[Table("BoardMembers")]
public class BoardMember
{
    [Key]
    public int Id { get; set; }

    public int BoardId { get; set; }

    public int UserId { get; set; }

    public string Role { get; set; } = "Editor";

    public DateTime JoinedAt { get; set; }
}
