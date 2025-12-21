using Refit;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IBoardsApi
{
    [Get("/api/boards/my-boards")]
    Task<IApiResponse<IEnumerable<BoardSummaryDto?>>> GetAllAsync();

    [Get("/api/boards/{id}")]
    public Task<IApiResponse<BoardDetailsDto?>> GetByIdAsync([Url] int id);
}
