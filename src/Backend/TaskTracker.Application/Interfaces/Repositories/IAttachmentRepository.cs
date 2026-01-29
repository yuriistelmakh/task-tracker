using System.Collections.Generic;
using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IAttachmentRepository : IRepository<Attachment, int>
{
    Task<IEnumerable<Attachment>> GetAllByTaskId(int taskId);
}
