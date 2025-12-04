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

    public async Task<int> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var column = new BoardColumn
        {
            Title = request.Title,
            BoardId = request.BoardId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.CreatedBy,
            Order = request.Order
        };

        var result = await uow.ColumnRepository.AddAsync(column);

        uow.Commit();

        return result;
    }
}
