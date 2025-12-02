namespace TaskTracker.Application.Interfaces;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
}
