using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.BlobStorage;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Attachments.Commands.DeleteAttachment;

public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBlobStorageService _blobStorageService;

    public DeleteAttachmentCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBlobStorageService blobStorageService)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var attachment = await uow.AttachmentRepository.GetAsync(request.AttachmentId);

        if (attachment is null)
        {
            return Result.NotFound("Attachment not found.");
        }

        var uri = new Uri(attachment.FileUrl);
        var segments = uri.Segments;
        var encodedBlobName = string.Join("", segments.Skip(2));
        var blobName = Uri.UnescapeDataString(encodedBlobName);

        var deleted = await _blobStorageService.DeleteBlobAsync(blobName, BlobContainerType.TaskAttachments);

        if (!deleted)
        {
            return Result.Failure($"Blob file '{blobName}' not found in storage.");
        }

        await uow.AttachmentRepository.DeleteAsync(request.AttachmentId);

        uow.Commit();

        return Result.Success();
    }
}
