using TaskTracker.Domain.DTOs.Boards;
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

    public async Task<IEnumerable<BoardSummaryDto>?> GetAllAsync()
    {
        var result = await _boardsApi.GetAllAsync();

        if (!result.IsSuccessful)
        {
            return null;
        }

        return result.Content;
    }

    public async Task<BoardDetailsDto?> GetAsync(int id)
    {
        var result = await _boardsApi.GetByIdAsync(id);

        if (!result.IsSuccessful)
        {
            return null;
        }

        return result.Content;
    }
}
