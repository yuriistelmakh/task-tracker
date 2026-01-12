using MediatR;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Boards.Commands.CreateBoard;

public class CreateBoardCommand : IRequest<int>
{
    public required string Title { get; set; } 

    public required string BackgroundColor { get; set; }

    public BoardVisibility Visibility { get; set; }

    public int CreatedBy { get; set; }
}
