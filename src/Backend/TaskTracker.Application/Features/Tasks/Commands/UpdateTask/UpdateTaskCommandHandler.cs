using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBoardNotificator _boardNotificator;

    public UpdateTaskCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBoardNotificator boardNotificator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _boardNotificator = boardNotificator;
    }

    public async Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var task = await uow.TaskRepository.GetAsync(request.Id);

        if (task is null)
        {
            return Result.Failure("Task was not found", ErrorType.NotFound);
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.Priority = request.Priority;
        task.DueDate = request.DueDate;
        task.AssigneeId = request.AssigneeId;
        task.IsComplete = request.IsComplete;
        task.UpdatedBy = request.UpdatedBy;
        task.UpdatedAt = DateTime.UtcNow;

        await uow.TaskRepository.UpdateAsync(task);

        uow.Commit();

        await _boardNotificator.TaskUpdatedAsync(request.BoardId, task.ToTaskSummaryDto());

        return Result.Success();
    }
}
