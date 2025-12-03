using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Columns.Commands.CreateBoardColumns;

public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, int>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public CreateColumnCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<int> Handle(CreateColumnCommand command, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var column = new BoardColumn
        {
            Title = command.Title,
            BoardId = command.BoardId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = command.CreatedBy,
            Order = command.Order
        };

        var result = await uow.ColumnRepository.AddAsync(column);

        uow.Commit();

        return result;
    }
}
