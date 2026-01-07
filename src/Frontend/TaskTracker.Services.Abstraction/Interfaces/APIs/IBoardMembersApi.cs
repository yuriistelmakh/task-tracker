using Refit;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IBoardMembersApi
{
    [Get("/api/boards/{boardId}/members")]
    public Task<IApiResponse<IEnumerable<MemberSummaryDto>>> GetAllAsync(int boardId);

    [Get("/api/boards/{boardId}/members/{userId}")]
    public Task<IApiResponse<MemberSummaryDto>> GetByIdAsync(int boardId, int userId);

    [Put("/api/boards/{boardId}/members/{userId}")]
    public Task<IApiResponse> UpdateRoleAsync(int boardId, int userId, [Body] UpdateBoardMemberRoleRequest request);

    [Post("/api/boards/{boardId}/members/invitations")]
    public Task<IApiResponse> SendInvitationsAsync(int boardId, [Body] SendInvitationsRequest request);

    [Post("/api/boards/{boardId}/members/invitations/accept")]
    public Task<IApiResponse> AcceptInvitationAsync(int boardId, [Body] AcceptInvitationRequest request);

    [Post("/api/boards/{boardId}/members/invitations/reject")]
    public Task<IApiResponse> RejectInvitationAsync(int boardId, [Body] RejectInvitationRequest request);
}
