using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Columns.Commands.ReorderColumnTasks;

public class ReorderColumnTasksCommandHandler : IRequestHandler<ReorderColumnTasksCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBoardNotificator _boardNotificator;

    public ReorderColumnTasksCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBoardNotificator boardNotificator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _boardNotificator = boardNotificator;
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

        await _boardNotificator.BoardChangedAsync(request.BoardId);

        return result;
    }
}
