using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Tasks.Commands.ChangeStatus;

public class ChangeTaskStatusCommandHandler : IRequestHandler<ChangeTaskStatusCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBoardNotificator _boardNotificator;

    public ChangeTaskStatusCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBoardNotificator boardNotificator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _boardNotificator = boardNotificator;
    }

    public async Task<Result> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var task = await uow.TaskRepository.GetAsync(request.Id);

        if (task is null)
        {
            return Result.Failure("Task not found.", ErrorType.NotFound);
        }

        task.IsComplete = request.IsComplete;
        task.UpdatedBy = request.UpdatedBy;
        task.UpdatedAt = DateTime.UtcNow;

        await uow.TaskRepository.UpdateAsync(task);

        uow.Commit();

        await _boardNotificator.TaskUpdatedAsync(request.BoardId, task.ToTaskSummaryDto());

        return Result.Success();
    }
}
