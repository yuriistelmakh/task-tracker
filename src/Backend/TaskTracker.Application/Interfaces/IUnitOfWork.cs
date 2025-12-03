using System;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBoardRepository BoardRepository { get; }
    IBoardColumnRepository BoardColumnRepository { get; }
    IBoardTaskRepository BoardTaskRepository { get; }
    IUserRepository UserRepository { get; }

    void Commit();
}
