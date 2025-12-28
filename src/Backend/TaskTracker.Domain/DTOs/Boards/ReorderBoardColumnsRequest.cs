using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Columns;
namespace TaskTracker.Domain.DTOs.Boards;

public class ReorderBoardColumnsRequest
{
    public int MovedColumnId { get; set; }

    public int BoardId { get; set; }

    public List<MoveColumnRequest> MoveColumnRequests { get; set; } = [];
}
