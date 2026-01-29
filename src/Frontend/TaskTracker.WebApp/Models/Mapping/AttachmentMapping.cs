using TaskTracker.Domain.DTOs.Attachments;
using TaskTracker.Domain.Helpers;

namespace TaskTracker.WebApp.Models.Mapping;

public static class AttachmentMapping
{
    public static AttachmentModel ToAttachmentModel(this AttachmentDto dto)
    {
        var fileType = FileTypeHelper.GetFileTypeFromExtension(dto.Name);

        return new AttachmentModel
        {
            Id = dto.Id,
            CreatedBy = dto.CreatedBy.ToUserSummaryModel(),
            CreatedAt = dto.CreatedAt,
            FileUrl = dto.FileUrl,
            Name = dto.Name,
            SizeKB = dto.SizeKB,
            Type = fileType
        };
    }
}
