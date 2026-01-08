using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.BoardMembers;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IBoardMembersService
{
    Task<Result> AcceptInvitationAsync(int boardId, AcceptInvitationRequest request);
    Task<Result<IEnumerable<MemberSummaryDto>>> GetAllAsync(int boardId);
    Task<Result<MemberSummaryDto>> GetByIdAsync(int boardId, int userId);
    Task<Result> RejectInvitationAsync(int boardId, RejectInvitationRequest request);
    Task<Result> SendInvitationAsync(int boardId, SendInvitationRequest request);
    Task<Result> UpdateRoleAsync(int boardId, int userId, UpdateBoardMemberRoleRequest request);
}
