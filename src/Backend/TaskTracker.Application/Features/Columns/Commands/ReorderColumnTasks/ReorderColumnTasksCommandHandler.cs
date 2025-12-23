using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Columns.Commands.ReorderColumnTasks;

public class ReorderColumnTasksCommandHandler : IRequestHandler<ReorderColumnTasksCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public ReorderColumnTasksCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(ReorderColumnTasksCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        foreach (var kvp in request.IdToOrder)
        {
            var isSuccessful = await uow.TaskRepository.UpdateOrderAsync(kvp.Key, kvp.Value);

            if (!isSuccessful)
            {
                return isSuccessful;
            }
        }

        var result = await uow.TaskRepository.MoveToColumn(request.TaskId, request.ColumnId);

        uow.Commit();

        return result;
    }
}
