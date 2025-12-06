namespace TaskTracker.Application.Interfaces.UoW;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
}
