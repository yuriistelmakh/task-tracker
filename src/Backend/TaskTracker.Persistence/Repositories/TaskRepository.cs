using Dapper;
using System.Data;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class TaskRepository : Repository<BoardTask, int>, IBoardTaskRepository
{
    public TaskRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public async Task<bool> UpdateStatusAsync(int id, bool status, int updatedBy)
    {
        var sql = @"
            UPDATE Tasks
            SET IsComplete = @Status,
            UpdatedBy = @UpdatedBy
            WHERE Id = @Id
        ";

        var result = await Connection.ExecuteAsync(sql, param: new { status, id, updatedBy }, transaction: Transaction);

        return result > 0;
    }
}
