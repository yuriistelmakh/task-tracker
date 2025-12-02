using Microsoft.Extensions.Configuration;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.Persistence.UoW;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IConfiguration _configuration;

    public UnitOfWorkFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IUnitOfWork Create()
    {
        return new UnitOfWork(_configuration);
    }
}
