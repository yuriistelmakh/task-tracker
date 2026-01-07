using Dapper;
using System.Collections.Generic;
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
        SELECT bm.*, u.*
        FROM BoardMembers bm
        JOIN Users u ON bm.UserId = u.Id
        WHERE bm.BoardId = @BoardId AND
              bm.UserId = @UserId
        ";

        var result = await Connection.QueryAsync<BoardMember, User, BoardMember>(
            sql,
            (member, user) =>
            {
                member.User = user;
                return member;
            },
            new { boardId, userId },
            splitOn: "Id",
            transaction: Transaction
        );

        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<BoardMember>> GetAllAsync(int boardId)
    {
        var sql = @"
            SELECT bm.*, u.*
            FROM BoardMembers bm
            JOIN Users u ON bm.UserId = u.Id
            WHERE bm.BoardId = @boardId";

        var result = await Connection.QueryAsync<BoardMember, User, BoardMember>(
            sql,
            (member, user) =>
            {
                member.User = user;
                return member;
            },
            new { boardId },
            splitOn: "Id",
            transaction: Transaction
        );

        return result;
    }
}
