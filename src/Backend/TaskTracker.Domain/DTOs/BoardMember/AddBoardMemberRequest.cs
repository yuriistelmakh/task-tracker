using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.BoardMember;

public class AddBoardMemberRequest
{
    public int UserId { get; set; }

    public BoardRoles Role { get; set; } = BoardRoles.Editor;
}
