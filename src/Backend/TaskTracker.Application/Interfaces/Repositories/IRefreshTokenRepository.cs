using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken, int>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
}
