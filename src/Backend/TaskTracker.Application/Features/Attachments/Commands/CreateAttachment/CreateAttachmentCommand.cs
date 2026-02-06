using MediatR;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace TaskTracker.Application.Features.Attachments.Commands.CreateAttachment;

public class CreateAttachmentCommand : IRequest<Result>
{
    public int TaskId { get; set; }

    public int CreatedBy { get; set; }

    public IFormFile File { get; set; }
}
