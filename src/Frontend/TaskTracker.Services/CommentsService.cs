using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Comments;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services;

public class CommentsService : ICommentsService
{
    private readonly ICommentsApi _commentsApi;

    public CommentsService(ICommentsApi commentsApi)
    {
        _commentsApi = commentsApi;
    }

    public async Task<Result<PagedResponse<CommentDto>>> GetAsync(int boardId, int taskId, int page, int pageSize)
    {
        var result = await _commentsApi.GetAsync(boardId, taskId, page, pageSize);

        return result.IsSuccessful
            ? Result<PagedResponse<CommentDto>>.Success(result.Content!)
            : Result<PagedResponse<CommentDto>>.Failure(result.Error.Message);
    }

    public async Task<Result<CommentDto>> CreateAsync(int boardId, int taskId, CreateCommentRequest request)
    {
        var result = await _commentsApi.CreateAsync(boardId, taskId, request);

        return result.IsSuccessful
            ? Result<CommentDto>.Success(result.Content)
            : Result<CommentDto>.Failure(result.Error.Message);
    }
}
