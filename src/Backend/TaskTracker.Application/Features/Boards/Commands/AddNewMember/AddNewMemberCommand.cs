using MediatR;

namespace TaskTracker.Application.Features.Boards.Commands.AddNewMember;

public class AddNewMemberCommand : IRequest<int>
{
    public int UserId { get; set; }

    public int BoardId { get; set; }

    public string Role { get; set; } = "Editor";
}
