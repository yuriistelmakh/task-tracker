using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTracker.Domain.Entities;

[Table("RefreshTokens")]
public class RefreshToken
{
    public int Id { get; set; }

    public required string Token { get; set; }

    public int UserId { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    [Editable(false)]
    public bool IsActive => RevokedAt == null && DateTime.UtcNow < ExpiresAt;
}
