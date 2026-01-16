using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Comments;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface ICommentsService
{
    Task<Result<CommentDto>> CreateAsync(int boardId, int taskId, CreateCommentRequest request);
    Task<Result<PagedResponse<CommentDto>>> GetAsync(int boardId, int taskId, int page, int pageSize); 
}
