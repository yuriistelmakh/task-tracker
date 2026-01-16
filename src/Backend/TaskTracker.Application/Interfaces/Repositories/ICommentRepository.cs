using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface ICommentRepository : IRepository<Comment, int>
{
    Task<IEnumerable<Comment>> GetAllAsync(int taskId, int page, int pageSize);
}
