using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class ColumnRepository : Repository<BoardColumn, int>, IColumnRepository
{
    public ColumnRepository(IDbTransaction transaction) : base(transaction)
    {
    }
}
