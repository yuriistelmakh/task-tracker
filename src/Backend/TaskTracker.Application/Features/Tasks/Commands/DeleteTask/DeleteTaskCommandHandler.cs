using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBoardNotificator _boardNotificator;

    public DeleteTaskCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBoardNotificator boardNotificator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _boardNotificator = boardNotificator;
    }

    public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        await uow.TaskRepository.DeleteAsync(request.Id);

        uow.Commit();

        await _boardNotificator.TaskDeletedAsync(request.BoardId, request.Id);

        return Result.Success();
    }
}
