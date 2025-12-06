using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.Entities;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public required string Tag { get; set; }

    public required string DisplayName { get; set; }

    public string? AvatarUrl { get; set; }

    public Roles Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
}
