using MediatR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Notifications;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Users.Queries.GetNotifications;

public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, IEnumerable<NotificationDto>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetUserNotificationsQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<IEnumerable<NotificationDto>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var notifications = await uow.NotificationRepository.GetAllUnreadAsync(request.UserId);

        uow.Commit();

        return notifications.Select(n => n.ToNotificationDto()) ?? [];
    }
}
