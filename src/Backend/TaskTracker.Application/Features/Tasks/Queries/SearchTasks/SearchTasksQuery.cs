using MediatR;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Application.Features.Tasks.Queries.SearchTasks;

public class SearchTasksQuery : IRequest<Result<IEnumerable<TaskSummaryDto>>>
{
    public int BoardId { get; set; }

    public string? Prompt { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }
}
