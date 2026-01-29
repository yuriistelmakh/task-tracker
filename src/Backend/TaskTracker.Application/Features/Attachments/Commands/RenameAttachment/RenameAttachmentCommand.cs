using MediatR;

namespace TaskTracker.Application.Features.Attachments.Commands.RenameAttachment;

public class RenameAttachmentCommand : IRequest<Result>
{
    public int Id { get; set; }

    public required string NewName { get; set; }
}
