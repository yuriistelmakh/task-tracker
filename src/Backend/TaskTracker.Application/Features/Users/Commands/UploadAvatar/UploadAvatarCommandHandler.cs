using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.BlobStorage;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Users.Commands.ChangeAvatar;

public class UploadAvatarCommandHandler : IRequestHandler<UploadAvatarCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBlobStorageService _blobStorageService;

    public UploadAvatarCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBlobStorageService blobStorageService)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var path = await _blobStorageService.UploadAvatarAsync(request.File, request.UserId);

        if (path is null)
        {
            return Result.Failure("Failed to upload a file to storage");
        }

        var user = await uow.UserRepository.GetAsync(request.UserId);

        if (user is null)
        {
            return Result.NotFound("User was not found");
        }

        user.AvatarUrl = path;

        await uow.UserRepository.UpdateAsync(user);

        uow.Commit();

        return Result.Success();
    }
}
