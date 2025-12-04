using Dapper;
using System.Data;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class UserRepository : Repository<User, int>, IUserRepository
{
    public UserRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public async Task<int> DeleteUser(int id)
    {
        var sql = @"
            UPDATE Users
            SET IsDeleted = 1
            WHERE Id = @Id
        ";

        return await Connection.ExecuteAsync(sql, transaction: Transaction, param: new { Id = id });
    }
}
