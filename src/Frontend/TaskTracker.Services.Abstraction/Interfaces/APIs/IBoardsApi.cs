using Refit;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IBoardsApi
{
    [Get("/api/boards/my-boards")]
    Task<IApiResponse<IEnumerable<BoardSummaryDto?>>> GetAllAsync();

    [Get("/api/boards/{id}")]
    public Task<IApiResponse<BoardDetailsDto?>> GetByIdAsync(int id);

    [Get("/api/boards/{id}/members")]
    public Task<IApiResponse<IEnumerable<UserSummaryDto>>> GetMembersAsync(int id);

    [Post("/api/boards/{id}/reorder")]
    public Task<IApiResponse> ReorderColumnsAsync(int id, ReorderBoardColumnsRequest request);
}
