using Dapper;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class UserRepository : Repository<User, int>, IUserRepository
{
    public UserRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public async Task<int> DeleteUserAsync(int id)
    {
        var sql = @"
            UPDATE Users
            SET IsDeleted = 1,
                Email = Users.Email + '_del_' + CAST(Id AS VARCHAR),
                Tag = Users.Tag + '_del_' + CAST(Id AS VARCHAR)
            WHERE Id = @Id
        ";

        return await Connection.ExecuteAsync(sql, transaction: Transaction, param: new { Id = id });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var sql = @"
            SELECT *
            FROM Users
            WHERE 
                Email = @Email
                AND IsDeleted = 0";

        var users = await Connection.QueryAsync<User>(sql, transaction: Transaction, param: new { email });

        return users.FirstOrDefault();
    }

    public async Task<User?> GetByTagAsync(string tag)
    {
        var sql = @"
            SELECT *
            FROM Users
            WHERE 
                Tag = @Tag
                AND IsDeleted = 0";

        var users = await Connection.QueryAsync<User>(sql, transaction: Transaction, param: new { tag });

        return users.FirstOrDefault();
    }
}
