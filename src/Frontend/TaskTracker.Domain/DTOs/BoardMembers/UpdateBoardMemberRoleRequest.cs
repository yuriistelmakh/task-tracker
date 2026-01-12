using TaskTracker.Domain.Enums;
namespace TaskTracker.Domain.DTOs.BoardMembers;

public class UpdateBoardMemberRoleRequest
{
    public BoardRole NewRole { get; set; }
}