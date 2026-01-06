using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.Users;

public class UpdateBoardMemberRoleRequest
{
    public BoardRole NewRole { get; set; }
}
