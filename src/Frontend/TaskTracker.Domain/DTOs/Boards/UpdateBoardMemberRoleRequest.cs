using TaskTracker.Domain.Enums;
namespace TaskTracker.Domain.DTOs.Boards;

public class UpdateBoardMemberRoleRequest
{
    public BoardRole NewRole { get; set; }
}