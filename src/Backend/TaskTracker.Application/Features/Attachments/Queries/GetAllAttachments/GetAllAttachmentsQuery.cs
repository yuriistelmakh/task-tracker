using MediatR;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Attachments;

namespace TaskTracker.Application.Features.Attachments.Queries.GetAllAttachments;

public class GetAllAttachmentsQuery : IRequest<Result<IEnumerable<AttachmentDto>>>
{
    public int TaskId { get; set; }
}
