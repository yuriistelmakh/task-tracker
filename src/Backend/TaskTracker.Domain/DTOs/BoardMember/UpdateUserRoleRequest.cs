using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.BoardMember;

public class UpdateUserRoleRequest
{
    public int UserId { get; set; }

    public int BoardId { get; set; }

    public BoardRole NewRole { get; set; }
}
