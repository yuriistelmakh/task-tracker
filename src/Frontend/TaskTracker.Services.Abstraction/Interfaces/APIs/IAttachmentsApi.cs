using Refit;
using TaskTracker.Domain.DTOs.Attachments;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IAttachmentsApi
{
    [Multipart]
    [Post("/api/boards/{boardId}/tasks/{taskId}/attachments")]
    public Task<IApiResponse> CreateAsync(
        int boardId,
        int taskId,
        int createdBy,
        StreamPart file
    );

    [Get("/api/boards/{boardId}/tasks/{taskId}/attachments")]
    public Task<IApiResponse<IEnumerable<AttachmentDto>>> GetAllAsync(int boardId, int taskId);

    [Delete("/api/boards/{boardId}/tasks/{taskId}/attachments/{attachmentId}")]
    public Task<IApiResponse> DeleteAsync(int boardId, int taskId, int attachmentId);

    [Patch("/api/boards/{boardId}/tasks/{taskId}/attachments/{attachmentId}/rename")]
    public Task<IApiResponse> RenameAsync(int boardId, int taskId, int attachmentId, [Body] RenameAttachmentRequest request);
}
