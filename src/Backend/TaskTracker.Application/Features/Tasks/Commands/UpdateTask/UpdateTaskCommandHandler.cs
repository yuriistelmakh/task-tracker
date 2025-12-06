using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public UpdateTaskCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var task = await uow.TaskRepository.GetAsync(request.Id);

        if (task is null)
        {
            return false;
        }

        task.ColumnId = request.ColumnId;
        task.Title = request.Title;
        task.Description = request.Description;
        task.Priority = request.Priority;
        task.DueDate = request.DueDate;
        task.Order = request.Order;
        task.AssigneeId = request.AssigneeId;
        task.IsComplete = request.IsComplete;
        task.UpdatedBy = request.UpdatedBy;
        task.UpdatedAt = DateTime.UtcNow;

        uow.Commit();

        return true;
    }
}
