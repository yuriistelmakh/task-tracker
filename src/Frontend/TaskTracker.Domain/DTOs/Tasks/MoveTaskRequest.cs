namespace TaskTracker.Domain.DTOs.Tasks;

public class MoveTaskRequest
{
    public int TaskId { get; set; }

    public int NewOrder { get; set; }
}
