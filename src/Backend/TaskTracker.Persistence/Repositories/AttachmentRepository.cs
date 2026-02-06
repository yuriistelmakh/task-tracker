using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class AttachmentRepository : Repository<Attachment, int>, IAttachmentRepository
{
    public AttachmentRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public async Task<IEnumerable<Attachment>> GetAllByTaskId(int taskId)
    {
        var sql = @"
            SELECT a.*, u.*
            FROM Attachments a
            JOIN ActiveUsers u ON u.Id = a.CreatedBy
            WHERE a.TaskId = @TaskId";

        var attachments = await Connection.QueryAsync<Attachment, User, Attachment>(
            sql,
            (attachment, user) =>
            {
                attachment.Creator = user;
                return attachment;
            },
            new { TaskId = taskId },
            splitOn: "Id",
            transaction: Transaction);

        return attachments ?? [];
    }
}
