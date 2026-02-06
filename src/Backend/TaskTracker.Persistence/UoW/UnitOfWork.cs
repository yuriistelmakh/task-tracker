using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Persistence.Repositories;

namespace TaskTracker.Persistence.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;

    private IBoardRepository? _boardRepository;
    private IColumnRepository? _columnRepository;
    private IBoardTaskRepository? _taskRepository;
    private IUserRepository? _userRepository;
    private IRefreshTokenRepository? _refreshTokenRepository;
    private IMemberRepository? _memberRepository;
    private IInvitationRepository? _invitationRepository;
    private INotificationRepository? _notificationRepository;
    private ICommentRepository? _commentRepository;
    private IAttachmentRepository? _attachmentRepository;

    public IBoardRepository BoardRepository =>
        _boardRepository ??= new BoardRepository(_transaction);

    public IColumnRepository ColumnRepository =>
        _columnRepository ??= new ColumnRepository(_transaction);

    public IBoardTaskRepository TaskRepository =>
        _taskRepository ??= new TaskRepository(_transaction);

    public IUserRepository UserRepository =>
        _userRepository ??= new UserRepository(_transaction);

    public IMemberRepository MemberRepository =>
        _memberRepository ??= new MemberRepository(_transaction);

    public IRefreshTokenRepository RefreshTokenRepository =>
        _refreshTokenRepository ??= new RefreshTokenRepository(_transaction);

    public IInvitationRepository InvitationRepository =>
        _invitationRepository ??= new InvitationRepository(_transaction);

    public INotificationRepository NotificationRepository =>
        _notificationRepository ??= new NotificationRepository(_transaction);

    public ICommentRepository CommentRepository =>
        _commentRepository ??= new CommentRepository(_transaction);

    public IAttachmentRepository AttachmentRepository =>
        _attachmentRepository ??= new AttachmentRepository(_transaction);


    public UnitOfWork(IConfiguration configuration)
    {
        _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        _connection.Open();

        _transaction = _connection.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            _transaction.Commit();
        }
        catch
        {
            _transaction.Rollback();
            throw;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }
}
