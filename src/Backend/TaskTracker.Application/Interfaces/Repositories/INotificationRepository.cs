using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface INotificationRepository : IRepository<Notification, int>
{
    Task<IEnumerable<Notification>> GetAllUnreadAsync(int userId);
}
