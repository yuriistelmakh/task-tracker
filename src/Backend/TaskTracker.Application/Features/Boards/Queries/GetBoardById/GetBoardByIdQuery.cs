using MediatR;
using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.Application.Features.Boards.Queries.GetBoardById;

public class GetBoardByIdQuery : IRequest<BoardDetailsDto?>
{
    public int Id { get; set; }

    public GetBoardByIdQuery(int id)
    {
        Id = id;
    }
}
