using MediatR;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Boards.Commands.SendInvitations;

public class SendInvitationsCommandHandler : IRequestHandler<SendInvitationsCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public SendInvitationsCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result> Handle(SendInvitationsCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        foreach (var inviteeId in request.InviteeIds)
        {
            Invitation invitation = new()
            {
                InviteeId = inviteeId,
                InviterId = request.InviterId,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                BoardId = request.BoardId,
            };

            var invitationId = await uow.InvitationRepository.AddAsync(invitation);

            if (invitationId == 0)
            {
                return Result.Conflict("User was already invited to this board.");
            }
            
            invitation.Id = invitationId;

            var board = await uow.BoardRepository.GetAsync(request.BoardId);

            if (board is null)
            {
                return Result.NotFound("Board was not found.");
            }

            var user = await uow.UserRepository.GetAsync(request.InviterId);

            if (user is null)
            {
                return Result.NotFound("User was not found");
            }

            var notification = new Notification
            {
                Title = "Incoming invitation",
                Message = $"{user.DisplayName} invited you to board {board.Title}",
                Type = NotificationType.BoardInvitation,
                Payload = JsonSerializer.Serialize(invitation),
                IsRead = false,
                UserId = inviteeId,
                CreatedAt = DateTime.UtcNow
            };

            await uow.NotificationRepository.AddAsync(notification);
        }
        
        uow.Commit();

        return Result.Success();
    }
}
