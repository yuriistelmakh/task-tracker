using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class CommentRepository : Repository<Comment, int>, ICommentRepository
{
    public CommentRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public async Task<IEnumerable<Comment>> GetAllAsync(int taskId, int page, int pageSize)
    {
        int offset = (page - 1) * pageSize;

        var sql = @"
            SELECT c.*, u.*
            FROM Comments c
            JOIN ActiveUsers u ON u.Id = c.CreatedBy
            WHERE c.TaskId = @TaskId
            ORDER BY c.CreatedAt DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var comments = await Connection.QueryAsync<Comment, User, Comment>(
            sql,
            (comment, user) =>
            {
                comment.Creator = user;
                return comment;
            },
            new { TaskId = taskId, Offset = offset, PageSize = pageSize },
            splitOn: "Id",
            transaction: Transaction);

        return comments ?? [];
    }
}
