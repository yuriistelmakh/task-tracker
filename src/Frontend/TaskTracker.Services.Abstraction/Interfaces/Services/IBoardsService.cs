using TaskTracker.Domain;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Services.Abstraction.Interfaces.Services;

public interface IBoardsService
{
    Task<Result<int>> CreateAsync(CreateBoardRequest request);

    Task<Result<IEnumerable<BoardSummaryDto>>> GetAllAsync();

    Task<Result<BoardDetailsDto>> GetAsync(int id);

    Task<Result<IEnumerable<MemberSummaryDto>>> GetMembersAsync(int id);

    Task<Result> ReorderColumnsAsync(int id, ReorderBoardColumnsRequest request);

    Task<Result> UpdateUserRoleAsync(int boardId, int userId, UpdateBoardMemberRoleRequest request);

    Task<Result> SendInvitationsAsync(int boardId, SendInvitationsRequest request);
    Task<Result> AcceptInvitationAsync(int boardId, AcceptInvitationRequest request);
    Task<Result> RejectInvitationAsync(int boardId, RejectInvitationRequest request);
}
