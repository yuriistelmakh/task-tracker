using System.Threading.Tasks;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User, int>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByTagAsync(string tag);
}
