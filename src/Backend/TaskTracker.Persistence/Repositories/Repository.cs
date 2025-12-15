using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Persistence.Repositories;

public class Repository<T, TId> : IRepository<T, TId> where T: class
{
    protected readonly IDbTransaction Transaction;

    protected IDbConnection Connection => Transaction.Connection!;

    public Repository(IDbTransaction transaction)
    {
        Transaction = transaction;
    }

    public virtual async Task<TId?> AddAsync(T entity)
    {
        return await Connection.InsertAsync<TId, T>(entity, transaction: Transaction);
    }

    public virtual async Task<int> DeleteAsync(TId id)
    {
        return await Connection.DeleteAsync<T>(id, transaction: Transaction);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Connection.GetListAsync<T>(new { }, transaction: Transaction);
    }

    public virtual async Task<T?> GetAsync(TId id)
    {
        return await Connection.GetAsync<T>(id, transaction: Transaction);
    }

    public virtual async Task<int> UpdateAsync(T entity)
    {
        return await Connection.UpdateAsync<T>(entity, transaction: Transaction);
    }
}
