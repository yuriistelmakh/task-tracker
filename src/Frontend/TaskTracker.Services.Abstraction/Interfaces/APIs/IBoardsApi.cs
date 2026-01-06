using Refit;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IBoardsApi
{
    [Get("/api/boards/my-boards")]
    Task<IApiResponse<IEnumerable<BoardSummaryDto?>>> GetAllAsync();

    [Post("/api/boards")]
    Task<IApiResponse<int>> CreateAsync(CreateBoardRequest request);

    [Get("/api/boards/{id}")]
    public Task<IApiResponse<BoardDetailsDto?>> GetByIdAsync(int id);

    [Get("/api/boards/{id}/members")]
    public Task<IApiResponse<IEnumerable<MemberSummaryDto>>> GetMembersAsync(int id);

    [Put("/api/boards/{boardId}/members/{userId}")]
    public Task<IApiResponse> UpdateBoardMemberRoleAsync(int boardId, int userId, [Body] UpdateBoardMemberRoleRequest request);

    [Post("/api/boards/{id}/reorder")]
    public Task<IApiResponse> ReorderColumnsAsync(int id, [Body] ReorderBoardColumnsRequest request);

    [Post("/api/boards/{boardId}/invites")]
    public Task<IApiResponse> SendInvitationsAsync(int boardId, [Body] SendInvitationsRequest request);

    [Post("/api/boards/{boardId}/invites/accept")]
    public Task<IApiResponse> AcceptInvitationAsync(int boardId, [Body] AcceptInvitationRequest request);

    [Post("/api/boards/{boardId}/invites/reject")]
    public Task<IApiResponse> RejectInvitationAsync(int boardId, [Body] RejectInvitationRequest request);
}
