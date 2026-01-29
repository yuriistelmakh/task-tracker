using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Attachments;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IAttachmentsService
{
    Task<Result> CreateAsync(int boardId, int taskId, int createdBy, Stream fileStream, string fileName, string contentType);
    Task<Result> DeleteAsync(int boardId, int taskId, int attachmentId);
    Task<Result<IEnumerable<AttachmentDto>>> GetAllAsync(int boardId, int taskId);
}
