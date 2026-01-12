using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.BoardMembers.Commands.RejectInvitation;

public class RejectInvitationCommandHandler : IRequestHandler<RejectInvitationCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public RejectInvitationCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var notification = await uow.NotificationRepository.GetAsync(request.NotificationId);
        var invitation = await uow.InvitationRepository.GetAsync(request.InvitationId);

        if (notification is null || invitation is null)
        {
            return false;
        }

        notification.IsRead = true;
        invitation.IsAnswered = true;

        var rowsAffected = await uow.NotificationRepository.UpdateAsync(notification);

        if (rowsAffected < 0)
        {
            return false;
        }

        rowsAffected = await uow.InvitationRepository.UpdateAsync(invitation);

        if (rowsAffected < 0)
        {
            return false;
        }

        uow.Commit();

        return true;
    }
}
