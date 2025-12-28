using TaskTracker.Domain.DTOs.Columns;

namespace TaskTracker.Domain.DTOs.Boards;

public class ReorderBoardColumnsRequest
{
    public List<MoveColumnRequest> MoveColumnRequests { get; set; } = [];
}
