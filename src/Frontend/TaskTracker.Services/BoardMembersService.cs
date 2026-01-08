using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;
using TaskTracker.Domain.DTOs.BoardMembers;

namespace TaskTracker.Services;

public class BoardMembersService : IBoardMembersService
{
    private readonly IBoardMembersApi _boardMembersApi;

    public BoardMembersService(IBoardMembersApi boardMembersApi)
    {
        _boardMembersApi = boardMembersApi;
    }

    public async Task<Result<IEnumerable<MemberSummaryDto>>> GetAllAsync(int boardId)
    {
        var result = await _boardMembersApi.GetAllAsync(boardId);

        return result.IsSuccessful
            ? Result<IEnumerable<MemberSummaryDto>>.Success(result.Content)
            : Result<IEnumerable<MemberSummaryDto>>.Failure(result.Error.Content!);
    }

    public async Task<Result<MemberSummaryDto>> GetByIdAsync(int boardId, int userId)
    {
        var result = await _boardMembersApi.GetByIdAsync(boardId, userId);

        return result.IsSuccessful
            ? Result<MemberSummaryDto>.Success(result.Content)
            : Result<MemberSummaryDto>.Failure(result.Error.Message);
    }

    public async Task<Result> UpdateRoleAsync(int boardId, int userId, UpdateBoardMemberRoleRequest request)
    {
        var result = await _boardMembersApi.UpdateRoleAsync(boardId, userId, request);

        return result.IsSuccessful
            ? Result.Success()
            : Result.Failure(result.Error.Content!);
    }

    public async Task<Result> AcceptInvitationAsync(int boardId, AcceptInvitationRequest request)
    {
        var result = await _boardMembersApi.AcceptInvitationAsync(boardId, request);

        return result.IsSuccessful
            ? Result.Success()
            : Result.Failure(result.Error.Content!);
    }

    public async Task<Result> SendInvitationAsync(int boardId, SendInvitationRequest request)
    {
        var result = await _boardMembersApi.SendInvitationAsync(boardId, request);

        return result.IsSuccessful
            ? Result.Success()
            : Result.Failure(result.Error.Content!);
    }

    public async Task<Result> RejectInvitationAsync(int boardId, RejectInvitationRequest request)
    {
        var result = await _boardMembersApi.RejectInvitationAsync(boardId, request);

        return result.IsSuccessful
            ? Result.Success()
            : Result.Failure(result.Error.Content!);
    }
}
