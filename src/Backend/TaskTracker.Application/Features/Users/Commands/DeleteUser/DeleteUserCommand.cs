using MediatR;

namespace TaskTracker.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
}
