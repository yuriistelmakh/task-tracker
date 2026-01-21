using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBoardNotificator _boardNotificator;

    public CreateTaskCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBoardNotificator boardNotificator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _boardNotificator = boardNotificator;
    }

    public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var task = new BoardTask
        {
            ColumnId = request.ColumnId,
            Title = request.Title,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.CreatedBy,
            Order = request.Order
        };

        var id = await uow.TaskRepository.AddAsync(task);

        uow.Commit();

        task.Id = id;

        await _boardNotificator.TaskCreatedAsync(request.BoardId, task.ToTaskSummaryDto());

        return id;
    }
}
