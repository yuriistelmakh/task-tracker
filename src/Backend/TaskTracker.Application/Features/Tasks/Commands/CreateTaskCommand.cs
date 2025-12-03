using MediatR;

namespace TaskTracker.Application.Features.Tasks.Commands;

public class CreateTaskCommand : IRequest<int>
{
    public int ColumnId { get; set; }

    public required string Title { get; set; }

    public int CreatedBy { get; set; }
}
