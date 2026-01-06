using MediatR;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Boards.Commands.UpdateBoardMemberRole;

public class UpdateBoardMemberRoleCommand : IRequest<bool>
{
    public int UserId { get; set; }

    public int BoardId { get; set; }

    public BoardRole NewRole { get; set; }
}
