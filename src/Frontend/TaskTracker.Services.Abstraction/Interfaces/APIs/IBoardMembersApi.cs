using Refit;
using TaskTracker.Domain.DTOs.BoardMembers;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.APIs;

public interface IBoardMembersApi
{
    [Get("/api/boards/{boardId}/members?page={page}&pageSize={pageSize}")]
    public Task<IApiResponse<IEnumerable<MemberSummaryDto>>> GetAllAsync(int boardId, int? page, int? pageSize);

    [Get("/api/boards/{boardId}/members/search?prompt={prompt}&page={page}&pageSize={pageSize}")]
    public Task<IApiResponse<IEnumerable<MemberSummaryDto>?>> SearchAsync(int boardId, string? prompt, int page, int pageSize);

    [Get("/api/boards/{boardId}/members/{userId}")]
    public Task<IApiResponse<MemberSummaryDto>> GetByIdAsync(int boardId, int userId);

    [Put("/api/boards/{boardId}/members/{userId}")]
    public Task<IApiResponse> UpdateRoleAsync(int boardId, int userId, [Body] UpdateBoardMemberRoleRequest request);

    [Post("/api/boards/{boardId}/members/invitations")]
    public Task<IApiResponse> SendInvitationAsync(int boardId, [Body] SendInvitationRequest request);

    [Post("/api/boards/{boardId}/members/invitations/accept")]
    public Task<IApiResponse> AcceptInvitationAsync(int boardId, [Body] AcceptInvitationRequest request);

    [Post("/api/boards/{boardId}/members/invitations/reject")]
    public Task<IApiResponse> RejectInvitationAsync(int boardId, [Body] RejectInvitationRequest request);
}
