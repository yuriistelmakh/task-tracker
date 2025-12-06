using MediatR;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<string?>
{
    public string? Email { get; set; }

    public string? Tag { get; set; }

    public required string Password { get; set; }
}
