namespace TaskTracker.Domain.DTOs.Boards;

public class BoardMemberStatisticsDto
{
    public int TotalMembers { get; set; }

    public int OwnersCount { get; set; }
    
    public int AdministratorsCount { get; set; }

    public int MembersCount { get; set; }

    public int VisitorsCount { get; set; }
}
