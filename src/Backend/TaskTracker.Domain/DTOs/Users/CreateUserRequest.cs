using System;

namespace TaskTracker.Domain.DTOs.Users;

public class CreateUserRequest
{
    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public required string Tag { get; set; }

    public required string DisplayName { get; set; }
}
