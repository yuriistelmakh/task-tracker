using MediatR;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<Result<UserDetailsDto>>
{
    public int Id { get; set; }
}
