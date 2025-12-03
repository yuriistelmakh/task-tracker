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

    public async Task<Board?> GetByIdDetailsAsync(int id)
    {
        var sql = @"
            SELECT b.*, u.*
            FROM Boards b
            JOIN Users u ON b.CreatedBy = u.Id
            WHERE b.Id = @Id;

            SELECT *
            FROM Columns
            WHERE BoardId = @Id
            ORDER BY [Order];

            SELECT t.*
            FROM Tasks t
            JOIN Columns c ON t.ColumnId = c.Id
            WHERE c.BoardId = @Id
            ORDER BY t.[Order];";

        using var multi = await Connection.QueryMultipleAsync(sql, new { Id = id }, transaction: Transaction);

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

    public async Task<IEnumerable<Board>> GetAllWithOwnersAsync()
    {
        var sql = @"
            SELECT b.*, u.* FROM Boards b
            JOIN Users u ON b.CreatedBy = u.id
            WHERE b.IsArchived = 0";

        var boards = await Connection.QueryAsync<Board, User, Board>(
            sql,
            (board, user) =>
            {
                board.Creator = user;
                return board;
            },
            transaction: Transaction,
            splitOn: "Id"
        );

        return boards;
    }
}
