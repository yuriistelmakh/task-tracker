namespace TaskTracker.WebApp.Models;

public class BoardVm
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public bool IsArchived { get; set; }

    public int TasksCount { get; set; }

    public int MembersCount { get; set; }

    public required string OwnerName { get; set; }

    public string? OwnerIconUrl { get; set; }
}
