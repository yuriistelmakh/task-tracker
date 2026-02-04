using MediatR;

namespace TaskTracker.Application.Features.Columns.Commands.CreateBoardColumns;

public class CreateColumnCommand : IRequest<Result<int>>
{
    public int BoardId { get; set; }

    public int CreatedBy { get; set; }

    public required string Title { get; set; }

    public int Order { get; set; }
}
