using TaskTracker.Domain.DTOs.Attachments;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Mapping;

public static class AttachmentMapping
{
    public static AttachmentDto ToAttachmentDto(this Attachment attachment) =>
        new()
        {
            Id = attachment.Id,
            CreatedBy = attachment.Creator.ToUserSummaryDto(),
            CreatedAt = attachment.CreatedAt,
            FileUrl = attachment.FileUrl,
            Name = attachment.Name,
            SizeKB = attachment.SizeKB
        };
}
