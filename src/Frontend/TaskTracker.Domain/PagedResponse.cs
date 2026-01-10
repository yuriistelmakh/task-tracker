namespace TaskTracker.Domain;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; } = [];

    public int TotalCount { get; set; }
}
