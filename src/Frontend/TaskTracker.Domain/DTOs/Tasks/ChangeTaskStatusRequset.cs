namespace TaskTracker.Domain.DTOs.Tasks;

public class ChangeTaskStatusRequest
{
    public bool IsComplete { get; set; }

    public int UpdatedBy { get; set; }
}
