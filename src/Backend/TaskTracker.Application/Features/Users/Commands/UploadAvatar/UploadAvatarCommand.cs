using MediatR;
using Microsoft.AspNetCore.Http;

namespace TaskTracker.Application.Features.Users.Commands.ChangeAvatar;

public class UploadAvatarCommand : IRequest<Result>
{
    public int UserId { get; set; }

    public IFormFile File { get; set; }
}
