using Dapper;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class MemberRepository : Repository<BoardMember, int>, IMemberRepository
{
    public MemberRepository(IDbTransaction transaction) : base(transaction)
    {

    }

    public async Task<BoardMember?> GetByIdsAsync(int boardId, int userId)
    {
        var sql = @"
            SELECT bm.*
            FROM BoardMembers bm
            WHERE BoardId = @BoardId AND
                  UserId = @UserId
        ";

        var result = await Connection.QueryAsync<BoardMember>(sql, param: new { boardId, userId }, transaction: Transaction);

        return result.FirstOrDefault();
    }
}
