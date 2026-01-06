using Dapper;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class NotificationRepository : Repository<Notification, int>, INotificationRepository
{
    public NotificationRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public async Task<IEnumerable<Notification>> GetAllUnreadAsync(int userId)
    {
        var sql = @"
            SELECT *
            FROM Notifications
            WHERE UserId = @UserId AND
                  IsRead = 0
        ";

        var notifications = await Connection.QueryAsync<Notification>(sql, param: new { userId }, transaction: Transaction);

        return notifications;
    }
}
