using MediatR;
using Microsoft.AspNetCore.Http;

namespace TaskTracker.Application.Features.Users.Commands.ChangeAvatar;

public class ChangeAvatarCommand : IRequest<Result>
{
    public int UserId { get; set; }

    public IFormFile File { get; set; }
}
