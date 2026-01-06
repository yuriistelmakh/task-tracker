using System;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.Interfaces.UoW;

public interface IUnitOfWork : IDisposable
{
    IBoardRepository BoardRepository { get; }
    IColumnRepository ColumnRepository { get; }
    IBoardTaskRepository TaskRepository { get; }
    IUserRepository UserRepository { get; }
    IMemberRepository MemberRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IInvitationRepository InvitationRepository { get; }
    INotificationRepository NotificationRepository { get; }
    void Commit();
}
