using MediatR;

namespace TaskTracker.Application.Features.Boards.Commands.DeleteBoard;

public class DeleteBoardCommand : IRequest<bool>
{
    public int Id { get; set; }
}
