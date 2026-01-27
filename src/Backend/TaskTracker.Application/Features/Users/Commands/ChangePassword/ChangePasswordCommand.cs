using MediatR;

namespace TaskTracker.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Result>
{
    public int UserId { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
