using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using TaskTracker.Application.Interfaces;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Persistence.Repositories;

namespace TaskTracker.Persistence.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;

    private IBoardRepository? _boardRepository;

    public IBoardRepository Boards =>
        _boardRepository ??= new BoardRepository(_transaction);

    public UnitOfWork(IConfiguration configuration)
    {
        _connection = new SqlConnection(configuration.GetConnectionString("TestConnection"));
        _connection.Open();

        _transaction = _connection.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            _transaction.Commit();
        }
        catch
        {
            _transaction.Rollback();
            throw;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }
}
