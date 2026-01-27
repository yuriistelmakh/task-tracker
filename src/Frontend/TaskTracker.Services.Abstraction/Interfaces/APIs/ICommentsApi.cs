using Refit;
using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Comments;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface ICommentsApi
{
    [Get("/api/boards/{boardId}/tasks/{taskId}/comments")]
    public Task<IApiResponse<PagedResponse<CommentDto?>>> GetAsync(int boardId, int taskId, int page, int pageSize);

    [Post("/api/boards/{boardId}/tasks/{taskId}/comments")]
    public Task<IApiResponse<CommentDto>> CreateAsync(int boardId, int taskId, [Body] CreateCommentRequest request);
}
