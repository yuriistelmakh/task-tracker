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
    private IBoardColumnRepository? _boardColumnRepository;
    private IBoardTaskRepository? _boardTaskRepository;
    private IUserRepository? _userRepository;

    public IBoardRepository BoardRepository =>
        _boardRepository ??= new BoardRepository(_transaction);

    public IBoardColumnRepository BoardColumnRepository =>
        _boardColumnRepository ??= new BoardColumnRepository(_transaction);

    public IBoardTaskRepository BoardTaskRepository =>
        _boardTaskRepository ??= new BoardTaskRepository(_transaction);

    public IUserRepository UserRepository =>
        _userRepository ??= new UserRepository(_transaction);

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
