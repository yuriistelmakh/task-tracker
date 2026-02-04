using MediatR;
using System.Collections.Generic;

namespace TaskTracker.Application.Features.Columns.Commands.ReorderColumnTasks;

public class ReorderColumnTasksCommand : IRequest<bool>
{
    public int BoardId { get; set; }

    public int TaskId { get; set; }

    public int ColumnId { get; set; }

    public Dictionary<int, int> IdToOrder { get; set; } = [];
}
