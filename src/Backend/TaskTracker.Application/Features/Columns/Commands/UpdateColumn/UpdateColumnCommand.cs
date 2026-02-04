using MediatR;

namespace TaskTracker.Application.Features.Columns.Commands.UpdateColumn;

public class UpdateColumnCommand : IRequest<Result>
{
    public int Id { get; set; }

    public int BoardId { get; set; }

    public required string Title { get; set; }

    public int Order { get; set; }

    public int UpdatedBy { get; set; }
}
