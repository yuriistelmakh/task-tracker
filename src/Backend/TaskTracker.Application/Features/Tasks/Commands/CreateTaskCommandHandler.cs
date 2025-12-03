using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Tasks.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public CreateTaskCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<int> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var task = new BoardTask
        {
            ColumnId = command.ColumnId,
            Title = command.Title,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = command.CreatedBy
        };

        var result = await uow.TaskRepository.AddAsync(task);

        uow.Commit();

        return result;
    }
}
