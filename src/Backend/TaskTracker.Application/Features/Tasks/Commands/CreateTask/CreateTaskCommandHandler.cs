using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public CreateTaskCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
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

        var result = await uow.TaskRepository.AddAsync(task);

        uow.Commit();

        return result;
    }
}
