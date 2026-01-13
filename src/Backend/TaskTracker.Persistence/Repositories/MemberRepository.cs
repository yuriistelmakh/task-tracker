using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.DTOs;
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

    public async Task<IEnumerable<BoardMember>> GetAllAsync(int boardId, int? page = null, int? pageSize = null)
    {
        var sqlBuilder = new StringBuilder(@"
            SELECT bm.*, u.*
            FROM BoardMembers bm
            JOIN Users u ON bm.UserId = u.Id
            WHERE bm.BoardId = @boardId");

        var parameters = new DynamicParameters();
        parameters.Add("boardId", boardId);

        sqlBuilder.Append(" ORDER BY u.DisplayName");

        if (page.HasValue && pageSize.HasValue)
        {
            sqlBuilder.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");

            int offset = (page.Value - 1) * pageSize.Value;

            parameters.Add("Offset", offset);
            parameters.Add("PageSize", pageSize.Value);
        }

        var result = await Connection.QueryAsync<BoardMember, User, BoardMember>(
            sqlBuilder.ToString(),
            (member, user) =>
            {
                member.User = user;
                return member;
            },
            parameters,
            splitOn: "Id",
            transaction: Transaction
        );

        return result;
    }

    public async Task<(IEnumerable<BoardMember> Items, int Count)> SearchByNameOrTag(int boardId, string? searchPrompt, int pageSize, int page = 1)
    {
        if (string.IsNullOrWhiteSpace(searchPrompt))
        {
            searchPrompt = null;
        }

        int offset = (page - 1) * pageSize;

        var sql = @"
            SELECT COUNT(*)
            FROM BoardMembers bm    
            JOIN Users u ON u.Id = bm.UserId 
            WHERE
                bm.BoardId = @boardId
                AND (
                    @SearchPrompt IS NULL 
                    OR u.DisplayName LIKE @SearchPrompt + '%' 
                    OR u.Tag LIKE @SearchPrompt + '%'
                );

            SELECT bm.*, u.*
            FROM BoardMembers bm    
            JOIN Users u ON u.Id = bm.UserId 
            WHERE
                bm.BoardId = @boardId
                AND (
                    @SearchPrompt IS NULL 
                    OR u.DisplayName LIKE @SearchPrompt + '%' 
                    OR u.Tag LIKE @SearchPrompt + '%'
                )
            ORDER BY u.DisplayName
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
        ";

        using var multi = await Connection.QueryMultipleAsync(
            sql,
            new
            {
                boardId,
                searchPrompt,
                Offset = offset,
                PageSize = pageSize
            },
            transaction: Transaction
        );

        var totalCount = await multi.ReadSingleAsync<int>();

        var items = multi.Read<BoardMember, User, BoardMember>(
            (member, user) =>
            {
                member.User = user;
                return member;
            },
            splitOn: "Id"
        );

        return (items, totalCount);
    }

    public async Task<int> GetCountAsync(int boardId)
    {
        var sql = @"
            SELECT COUNT(*)
            FROM BoardMembers
            WHERE BoardId = @BoardId
        ";

        var count = await Connection.ExecuteScalarAsync<int>(sql, new { boardId }, transaction: Transaction);

        return count;
    }
}
