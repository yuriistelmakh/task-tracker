using Dapper;
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

    public async Task<bool> UpdateOrderAsync(int id, int order)
    {
        var sql = @"
            UPDATE Columns
            SET [Order] = @Order
            WHERE Id = @Id
        ";

        var result = await Connection.ExecuteAsync(sql, param: new { order, id }, transaction: Transaction);

        return result > 0;
    }
}
