using MediatR;
using TaskTracker.Domain.DTOs.Auth;

namespace TaskTracker.Application.Features.Auth.Commands.Signup;

public class SignupCommand : IRequest<AuthResponse>
{
    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string Tag { get; set; }

    public required string DisplayName { get; set; }
}
