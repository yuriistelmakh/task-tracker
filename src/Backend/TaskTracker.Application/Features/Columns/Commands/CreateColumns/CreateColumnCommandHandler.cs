using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Columns.Commands.CreateBoardColumns;

public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, Result<int>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBoardNotificator _boardNotificator;

    public CreateColumnCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBoardNotificator boardNotificator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _boardNotificator = boardNotificator;
    }

    public async Task<Result<int>> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
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

        var id = await uow.ColumnRepository.AddAsync(column);

        uow.Commit();

        column.Id = id;

        await _boardNotificator.ColumnCreatedAsync(request.BoardId, column.ToColumnSummaryDto());

        return Result<int>.Success(id);
    }
}
