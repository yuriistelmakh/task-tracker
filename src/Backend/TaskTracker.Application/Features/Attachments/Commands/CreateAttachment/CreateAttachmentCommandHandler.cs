using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.BlobStorage;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Attachments.Commands.CreateAttachment;

public class CreateAttachmentCommandHandler : IRequestHandler<CreateAttachmentCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBlobStorageService _blobStorageService;

    public CreateAttachmentCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBlobStorageService blobStorageService)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result> Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var url = await _blobStorageService.UploadTaskAttachmentAsync(request.File, request.TaskId);

        if (string.IsNullOrEmpty(url))
        {
            return Result.Failure("Failed to upload the attachment to storage");
        }

        var attachment = new Attachment
        {
            Name = request.File.FileName,
            TaskId = request.TaskId,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            FileUrl = url,
            SizeKB = request.File.Length / 1024.0
        };

        var id = await uow.AttachmentRepository.AddAsync(attachment);

        uow.Commit();

        if (id == 0)
        {
            return Result.Failure("Failed to add an attachment");
        }

        return Result.Success();
    }
}
