using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class TaskRepository : Repository<BoardTask, int>, IBoardTaskRepository
{
    public TaskRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public async Task<BoardTask> GetByIdWithAttachments(int taskId)
    {
        var sql = @"
            SELECT t.*, a.*, u.*
            FROM Tasks t
            LEFT JOIN Attachments a ON a.TaskId = t.Id
            LEFT JOIN ActiveUsers u ON u.Id = a.CreatedBy
            WHERE t.Id = @TaskId
        ";

        var taskDict = new Dictionary<int, BoardTask>();

        await Connection.QueryAsync<BoardTask, Attachment, User, BoardTask>(
            sql,
            (task, attachment, user) =>
            {
                if (!taskDict.TryGetValue(task.Id, out var existingTask))
                {
                    existingTask = task;
                    existingTask.Attachments = new List<Attachment>();
                    taskDict.Add(task.Id, existingTask);
                }

                if (attachment is not null)
                {
                    attachment.Creator = user;
                    existingTask.Attachments.Add(attachment);
                }

                return existingTask;
            },
            new { TaskId = taskId },
            splitOn: "Id,Id",
            transaction: Transaction
        );

        return taskDict.Values.FirstOrDefault();
    }

    public async Task<IEnumerable<BoardTask>> GetAllByAssignee(int assigneeId)
    {
        var sql = @"
            SELECT *
            FROM Tasks
            WHERE AssigneeId = @AssigneeId
        ";

        var result = await Connection.QueryAsync<BoardTask>(sql, param: new { assigneeId }, transaction: Transaction);

        return result;
    }

    public async Task<bool> UpdateOrderAsync(int id, int order)
    {
        var sql = @"
            UPDATE Tasks
            SET [Order] = @Order
            WHERE Id = @Id
        ";

        var result = await Connection.ExecuteAsync(sql, param: new { order, id }, transaction: Transaction);

        return result > 0;
    }

    public async Task<bool> MoveToColumn(int id, int columnId)
    {
        var sql = @"
            UPDATE Tasks
            SET ColumnId = @ColumnId
            WHERE Id = @Id
        ";

        var result = await Connection.ExecuteAsync(sql, param: new { id, columnId }, transaction: Transaction);

        return result > 0;
    }

    public async Task<IEnumerable<BoardTask>> SearchAsync(int boardId, string? prompt, int pageSize)
    {
        var sql = @"
            SELECT TOP (@PageSize) t.*
            FROM Tasks t
            JOIN Columns c ON c.Id = t.ColumnId
            WHERE
                c.BoardId = @BoardId AND
                (@Prompt IS NULL OR t.Title LIKE @Prompt + '%')
            ORDER BY t.Title
        ";

        var tasks = await Connection.QueryAsync<BoardTask>(sql, param: new { boardId, prompt, pageSize }, transaction: Transaction);

        return tasks ?? [];
    }
}
