using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services;

public class BoardsService : IBoardsService
{
    private readonly IBoardsApi _boardsApi;

    public BoardsService(IBoardsApi boardsApi)
    {
        _boardsApi = boardsApi;
    }

    public async Task<Result<IEnumerable<BoardSummaryDto>>> GetAllAsync()
    {
        var result = await _boardsApi.GetAllAsync();

        return result.IsSuccessful
            ? Result<IEnumerable<BoardSummaryDto>>.Success(result.Content)
            : Result<IEnumerable<BoardSummaryDto>>.Failure(result.Error.Message);
    }

    public async Task<Result<BoardDetailsDto>> GetAsync(int id)
    {
        var result = await _boardsApi.GetByIdAsync(id);

        return result.IsSuccessful
            ? Result<BoardDetailsDto>.Success(result.Content)
            : Result<BoardDetailsDto>.Failure(result.Error.Message);
    }

    public async Task<Result<IEnumerable<UserSummaryDto>>> GetMembersAsync(int id)
    {
        var result = await _boardsApi.GetMembersAsync(id);

        return result.IsSuccessful
            ? Result<IEnumerable<UserSummaryDto>>.Success(result.Content)
            : Result<IEnumerable<UserSummaryDto>>.Failure(result.Error.Message);
    }

    public async Task<Result> ReorderColumnsAsync(int id, ReorderBoardColumnsRequest request)
    {
        var result = await _boardsApi.ReorderColumnsAsync(id, request); 

        return result.IsSuccessful
            ? Result.Success()
            : Result.Failure(result.Error.Message);
    }
}
