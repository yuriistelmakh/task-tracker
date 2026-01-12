using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.BoardMembers.Commands.AcceptInvitation;

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public AcceptInvitationCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var boardMember = new BoardMember
        {
            BoardId = request.BoardId,
            Role = request.Role,
            UserId = request.InviteeId,
            JoinedAt = DateTime.UtcNow
        };

        await uow.MemberRepository.AddAsync(boardMember);

        var notification = await uow.NotificationRepository.GetAsync(request.NotificationId);

        if (notification is null)
        {
            return Result.NotFound("Notification was not found.");
        }

        var invitation = await uow.InvitationRepository.GetAsync(request.InvitationId);

        if (invitation is null)
        {
            return Result.NotFound("Invitation was not found.");
        }

        notification.IsRead = true;
        invitation.IsAnswered = true;

        var result = await uow.NotificationRepository.UpdateAsync(notification);

        if (result == 0)
        {
            return Result.Failure("Failed to update the notfication.");
        }

        result = await uow.InvitationRepository.UpdateAsync(invitation);

        if (result == 0)
        {
            return Result.Failure("Failed to update the invitation.");
        }

        uow.Commit();

        return Result.Success();
    }
}
