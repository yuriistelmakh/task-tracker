using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;
using System.Linq;

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

    public async Task<IEnumerable<Board>> GetAllWithDetailsAsync(int userId)
    {
        var sqlBoards = @"
            SELECT b.*, u.* 
            FROM Boards b
            JOIN BoardMembers bm ON b.Id = bm.BoardId
            JOIN Users u ON b.CreatedBy = u.Id
            WHERE b.IsArchived = 0
              AND bm.UserId = @UserId";

        var boards = (await Connection.QueryAsync<Board, User, Board>(
            sqlBoards,
            (board, user) =>
            {
                board.Creator = user;
                return board;
            },
            param: new { userId },
            splitOn: "Id",
            transaction: Transaction
        )).ToList();

        if (!boards.Any())
            return boards;

        await LoadBoardDetailsAsync(boards);

        return boards;
    }

    private async Task LoadBoardDetailsAsync(List<Board> boards)
    {
        var boardIds = boards.Select(b => b.Id).ToArray();

        var sqlColumns = @"
            SELECT * 
            FROM Columns
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
            SELECT m.*, u.*
            FROM BoardMembers m
            JOIN Users u ON m.UserId = u.Id
            WHERE m.BoardId IN @BoardIds";

        var members = (await Connection.QueryAsync<BoardMember, User, BoardMember>(
            sqlMembers,
            (member, user) =>
            {
                member.User = user;
                return member;
            },
            new { BoardIds = boardIds },
            splitOn: "Id",
            transaction: Transaction
        )).ToList();

        var columnsByBoard = columns.ToLookup(c => c.BoardId);
        var membersByBoard = members.ToLookup(m => m.BoardId);

        foreach (var board in boards)
        {
            board.Columns = columnsByBoard[board.Id].ToList();
            board.Members = membersByBoard[board.Id].ToList();
        }
    }

    public async Task<int> AddMemberAsync(BoardMember boardMember)
    {
        return await Connection.InsertAsync<int, BoardMember>(boardMember, transaction: Transaction);
    }

    public async Task<IEnumerable<BoardMember>> GetMembersAsync(int boardId)
    {
        var sql = @"
            SELECT m.*, u.*
            FROM BoardMembers m
            JOIN Users u ON m.UserId = u.Id
            WHERE m.BoardId = @boardId";

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
