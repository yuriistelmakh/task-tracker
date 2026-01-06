using MediatR;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Notifications;

namespace TaskTracker.Application.Features.Users.Queries.GetNotifications;

public class GetUserNotificationsQuery : IRequest<IEnumerable<NotificationDto>>
{
    public int UserId { get; set; }
}
