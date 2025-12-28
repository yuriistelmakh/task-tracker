using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Tasks;

public class ReorderColumnTasksRequest
{
    // Ids and orders of all tasks in affected columns
    public List<MoveTaskRequest> MoveTaskRequests { get; set; } = [];

    // Id of the moved task to change its columnId
    public int MovedTaskId { get; set; }

    public int ColumnId { get; set; }
}