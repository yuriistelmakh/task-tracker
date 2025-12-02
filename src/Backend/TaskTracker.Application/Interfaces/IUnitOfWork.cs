using System;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBoardRepository BoardRepository { get; }

    void Commit();
}
