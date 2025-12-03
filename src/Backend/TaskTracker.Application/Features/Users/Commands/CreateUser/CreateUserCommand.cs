using MediatR;
using System;

namespace TaskTracker.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<int>
{
    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public required string Tag { get; set; }

    public required string DisplayName { get; set; }
}
