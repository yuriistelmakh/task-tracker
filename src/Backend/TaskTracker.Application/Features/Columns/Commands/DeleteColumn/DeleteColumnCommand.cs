using MediatR;

namespace TaskTracker.Application.Features.Columns.Commands.DeleteColumn;

public class DeleteColumnCommand : IRequest<Result>
{
    public int Id { get; set; }

    public int BoardId { get; set; }
}
