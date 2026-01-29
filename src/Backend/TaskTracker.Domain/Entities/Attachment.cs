using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Domain.Entities;

[Table("Attachments")]
public class Attachment
{
    [Key]
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int CreatedBy { get; set; }
    public User Creator { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public required string FileUrl { get; set; }

    public required string Name { get; set; }

    public double SizeKB { get; set; }
}
