using MediatR;

namespace TaskTracker.Application.Features.Boards.Commands.UpdateBoard;

public class UpdateBoardCommand : IRequest<bool>
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public string Description { get; set; } = string.Empty;

    public int UpdatedBy { get; set; }

    public bool IsArchived { get; set; }
}
