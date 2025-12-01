using MediatR;

namespace TaskTracker.Application.Features.Boards.Commands.CreateBoard
{
    public class CreateBoardCommand : IRequest<int>
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
