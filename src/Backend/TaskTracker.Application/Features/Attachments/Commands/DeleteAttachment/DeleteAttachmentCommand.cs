using MediatR;

namespace TaskTracker.Application.Features.Attachments.Commands.DeleteAttachment;

public class DeleteAttachmentCommand : IRequest<Result>
{
    public int AttachmentId { get; set; }
}
