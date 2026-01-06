using MediatR;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Boards.Commands.AddNewMember;

public class AddNewMemberCommand : IRequest<int>
{
    public int UserId { get; set; }

    public int BoardId { get; set; }

    public BoardRole Role { get; set; } = BoardRole.Member;
}
