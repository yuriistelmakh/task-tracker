using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IColumnRepository : IRepository<BoardColumn, int>
{
}
