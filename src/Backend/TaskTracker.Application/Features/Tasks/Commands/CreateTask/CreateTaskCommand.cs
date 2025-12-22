using MediatR;

namespace TaskTracker.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommand : IRequest<int>
{
    public int ColumnId { get; set; }

    public required string Title { get; set; }

    public int CreatedBy { get; set; }

    public int Order { get; set; }
}
