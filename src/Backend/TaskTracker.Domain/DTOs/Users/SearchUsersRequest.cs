namespace TaskTracker.Domain.DTOs.Users;

public class SearchUsersRequest
{
    public string? SearchPrompt { get; set; }

    public int Limit { get; set; }
}
