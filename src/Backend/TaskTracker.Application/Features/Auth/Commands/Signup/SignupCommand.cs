using MediatR;

namespace TaskTracker.Application.Features.Auth.Commands.Signup;

public class SignupCommand : IRequest<string>
{
    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string Tag { get; set; }

    public required string DisplayName { get; set; }
}
