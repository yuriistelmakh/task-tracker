using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace TaskTracker.Persistence.Repositories;

public class BoardRepository : Repository<Board, int>, IBoardRepository
{
    public BoardRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public async Task<Board?> GetDetailsAsync(int id)
    {
        var sql = @"
            SELECT b.*, u.*
            FROM Boards b
            JOIN Users u ON b.CreatedBy = u.Id
            WHERE b.Id = @Id

            SELECT *
            FROM Columns
            WHERE BoardId = @Id
            ORDER BY [Order];

            SELECT t.*
            FROM Tasks t
            JOIN Columns c ON t.ColumnId = c.Id
            WHERE c.BoardId = @Id
            ORDER BY t.[Order];";

        using var multi = await Connection.QueryMultipleAsync(sql, new { id }, transaction: Transaction);

        var board = multi.Read<Board, User, Board>((board, user) =>
        {
            board.Creator = user;
            return board;
        }).FirstOrDefault();

        if (board is not null)
        {
            var columns = await multi.ReadAsync<BoardColumn>();

            var tasks = await multi.ReadAsync<BoardTask>();
            var tasksByColumn = tasks.ToLookup(t => t.ColumnId);

            foreach (var column in columns)
            {
                column.Tasks = tasksByColumn[column.Id].ToList();
            }

            board.Columns = columns.ToList();
        }

        return board;
    }

    public async Task<IEnumerable<Board>> GetAllWithDetailsAsync(int userId, int page, int pageSize)
    {
        var sqlBoards = @"
            SELECT b.*, u.* FROM Boards b
            JOIN BoardMembers bm ON b.Id = bm.BoardId
            JOIN Users u ON b.CreatedBy = u.Id
            WHERE b.IsArchived = 0
              AND bm.UserId = @UserId
            ORDER BY b.CreatedAt DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        var offset = (page - 1) * pageSize;

        var boards = (await Connection.QueryAsync<Board, User, Board>(
            sqlBoards,
            (board, user) =>
            {
                board.Creator = user;
                return board;
            },
            param: new
            {
                UserId = userId,
                Offset = offset,
                PageSize = pageSize
            },
            splitOn: "Id",
            transaction: Transaction
        )).ToList();

        if (boards.Count == 0)
            return boards;

        await LoadBoardDetailsAsync(boards);

        return boards;
    }

    private async Task LoadBoardDetailsAsync(List<Board> boards)
    {
        var boardIds = boards.Select(b => b.Id).ToArray();

        var sqlColumns = @"
        SELECT * FROM Columns
        WHERE BoardId IN @BoardIds
        ORDER BY [Order];";

        var columns = (await Connection.QueryAsync<BoardColumn>(
            sqlColumns,
            new { BoardIds = boardIds },
            transaction: Transaction
        )).ToList();

        var columnIds = columns.Select(c => c.Id).ToArray();

        if (columnIds.Length != 0)
        {
            var sqlTasks = @"
            SELECT *
            FROM Tasks
            WHERE ColumnId IN @ColumnIds
            ORDER BY [Order];";

            var tasks = (await Connection.QueryAsync<BoardTask>(
                sqlTasks,
                new { ColumnIds = columnIds },
                transaction: Transaction
            )).ToList();

            var tasksByColumn = tasks.ToLookup(t => t.ColumnId);

            foreach (var column in columns)
            {
                column.Tasks = tasksByColumn[column.Id].ToList();
            }
        }

        var sqlMembers = @"
        SELECT m.BoardId, u.* FROM BoardMembers m
        JOIN Users u ON m.UserId = u.Id
        WHERE m.BoardId IN @BoardIds";

        var membersResult = (await Connection.QueryAsync<int, User, (int BoardId, User User)>(
            sqlMembers,
            (boardId, user) => (boardId, user),
            new { BoardIds = boardIds },
            splitOn: "Id",
            transaction: Transaction
        )).ToList();

        var columnsByBoard = columns.ToLookup(c => c.BoardId);
        var membersByBoard = membersResult.ToLookup(x => x.BoardId, x => x.User);

        foreach (var board in boards)
        {
            board.Columns = columnsByBoard[board.Id].ToList();
            board.Members = membersByBoard[board.Id].ToList();
        }
    }

    public async Task<int> GetMembersCountAsync(int id)
    {
        var sql = @"
            SELECT COUNT(*)
            FROM BoardMembers
            WHERE BoardId = @BoardId
        ";

        var count = await Connection.ExecuteScalarAsync<int>(sql, new { BoardId = id }, transaction: Transaction);

        return count;
    }

    public async Task<int> GetCountAsync(int userId)
    {
        var sql = @"
            SELECT COUNT(*)
            FROM Boards b
            JOIN BoardMembers bm ON b.Id = bm.BoardId
            WHERE bm.UserId = @UserId
        ";

        var count = await Connection.ExecuteScalarAsync<int>(sql, new { userId }, transaction: Transaction);

        return count;
    }
}
