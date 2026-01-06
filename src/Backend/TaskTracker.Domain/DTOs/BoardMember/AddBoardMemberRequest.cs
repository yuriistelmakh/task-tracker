using TaskTracker.Domain.Enums;

namespace TaskTracker.Domain.DTOs.BoardMember;

public class AddBoardMemberRequest
{
    public int UserId { get; set; }

    public BoardRole Role { get; set; } = BoardRole.Member;
}
