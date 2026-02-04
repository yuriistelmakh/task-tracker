using TaskTracker.WebApp.Models.Users;

namespace TaskTracker.WebApp.Models;

public class BoardModel
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public required string BackgroundColor { get; set; }

    public bool IsArchived { get; set; }

    public int TasksCount { get; set; }

    public int MembersCount { get; set; }

    public List<UserSummaryModel> Members { get; set; } = [];
}
