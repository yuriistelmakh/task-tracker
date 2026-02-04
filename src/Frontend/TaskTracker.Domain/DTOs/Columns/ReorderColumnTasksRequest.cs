using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Domain.DTOs.Columns;

public class ReorderColumnTasksRequest
{
    public int MovedTaskId { get; set; }

    public List<MoveTaskRequest> MoveTaskRequests { get; set; } = [];
}
