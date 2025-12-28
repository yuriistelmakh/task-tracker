using System.Threading.Tasks;
using System.Transactions;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IColumnRepository : IRepository<BoardColumn, int>
{
    Task<bool> UpdateOrderAsync(int id, int order);
}
