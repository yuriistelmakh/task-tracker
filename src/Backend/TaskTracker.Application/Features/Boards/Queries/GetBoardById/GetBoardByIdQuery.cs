using MediatR;
using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Features.Boards.Queries.GetBoardById
{
    public class GetBoardByIdQuery : IRequest<BoardDto?>
    {
        public int Id { get; set; }

        public GetBoardByIdQuery(int id)
        {
            Id = id;
        }
    }
}
