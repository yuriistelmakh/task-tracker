using Refit;
using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Attachments;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services;

public class AttachmentsService : IAttachmentsService
{
    private readonly IAttachmentsApi _attachmentsApi;

    public AttachmentsService(IAttachmentsApi attachmentsApi)
    {
        _attachmentsApi = attachmentsApi;
    }

    public async Task<Result> CreateAsync(
        int boardId,
        int taskId,
        int createdBy,
        Stream fileStream, 
        string fileName, 
        string contentType)
    {
        var file = new StreamPart(fileStream, fileName, contentType);
        var result = await _attachmentsApi.CreateAsync(boardId, taskId, createdBy, file);

        return result.IsSuccessful
            ? Result.Success()
            : Result.Failure(result.Error.Content!);
    }

    public async Task<Result<IEnumerable<AttachmentDto>>> GetAllAsync(int boardId, int taskId)
    {
        var result = await _attachmentsApi.GetAllAsync(boardId, taskId);

        return result.IsSuccessful
            ? Result<IEnumerable<AttachmentDto>>.Success(result.Content)
            : Result<IEnumerable<AttachmentDto>>.Failure(result.Error.Content!);
    }

    public async Task<Result> DeleteAsync(int boardId, int taskId, int attachmentId)
    {
        var result = await _attachmentsApi.DeleteAsync(boardId, taskId, attachmentId);

        return result.IsSuccessful
            ? Result.Success()
            : Result.Failure(result.Error.Content!);
    }
}