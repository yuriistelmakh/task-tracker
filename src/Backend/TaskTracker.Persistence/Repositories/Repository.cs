using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Persistence.Repositories;

public class Repository<T, TId> : IRepository<T, TId> where T: class
{
    protected readonly IDbTransaction _transaction;

    protected IDbConnection _connection => _transaction.Connection!;

    public Repository(IDbTransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task<TId> AddAsync(T entity)
    {
        return await _connection.InsertAsync<TId, T>(entity, transaction: _transaction);
    }

    public async Task DeleteAsync(TId id)
    {
        await _connection.DeleteAsync(id, transaction: _transaction);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _connection.GetListAsync<T>(new { }, transaction: _transaction);
    }

    public async Task<T?> GetAsync(TId id)
    {
        return await _connection.GetAsync<T>(id, transaction: _transaction);
    }

    public async Task UpdateAsync(T entity)
    {
        await _connection.UpdateAsync<T>(entity, transaction: _transaction);
    }
}
