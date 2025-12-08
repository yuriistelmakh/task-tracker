using MediatR;

namespace TaskTracker.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<bool>
{
    public int Id { get; set; }

    public required string Tag { get; set; }

    public required string PasswordHash { get; set; }

    public required string DisplayName { get; set; }

    public required string AvatarUrl { get; set; }
}
