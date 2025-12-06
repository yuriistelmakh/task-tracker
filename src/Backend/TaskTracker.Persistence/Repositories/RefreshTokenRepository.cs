using Dapper;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Persistence.Repositories;

public class RefreshTokenRepository : Repository<RefreshToken, int>, IRefreshTokenRepository
{
    public RefreshTokenRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        var sql = @"
            SELECT *
            FROM RefreshTokens
            WHERE Token = @Token";

        var refreshToken = await Connection.QueryAsync<RefreshToken>(sql, new { token }, transaction: Transaction);

        return refreshToken.FirstOrDefault();
    }
}
