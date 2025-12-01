using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Persistence.Repositories
{
    public class Repository<T, TId> : IRepository<T, TId> where T: class
    {
        private readonly string _connectionString;

        public Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<TId> AddAsync(T entity)
        {
            using var connection = CreateConnection();

            return await connection.InsertAsync<TId, T>(entity);
        }

        public async Task DeleteAsync(TId id)
        {
            using var connection = CreateConnection();
            await connection.DeleteAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.GetListAsync<T>();
        }

        public async Task<T?> GetAsync(TId id)
        {
            using var connection = CreateConnection();
            return await connection.GetAsync<T>(id);
        }

        public async Task UpdateAsync(T entity)
        {
            using var connection = CreateConnection();
            await connection.UpdateAsync<T>(entity);
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
