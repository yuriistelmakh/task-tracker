using MediatR;

namespace TaskTracker.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<Result>
{
    public int Id { get; set; }

    public required string Email { get; set; }

    public required string Tag { get; set; }

    public required string DisplayName { get; set; }
}
