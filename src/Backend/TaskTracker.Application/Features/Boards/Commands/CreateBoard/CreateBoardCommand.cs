using MediatR;

namespace TaskTracker.Application.Features.Boards.Commands.CreateBoard;

public class CreateBoardCommand : IRequest<int>
{
    public required string Title { get; set; } 

    public string Description { get; set; } = string.Empty;

    public int CreatedBy { get; set; }
}
