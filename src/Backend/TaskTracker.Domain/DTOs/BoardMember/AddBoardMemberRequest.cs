namespace TaskTracker.Domain.DTOs.BoardMember;

public class AddBoardMemberRequest
{
    public int UserId { get; set; }

    public string Role { get; set; } = "Editor";
}
