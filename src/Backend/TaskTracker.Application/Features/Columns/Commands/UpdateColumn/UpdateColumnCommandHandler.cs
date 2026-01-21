using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Columns.Commands.UpdateColumn;

public class UpdateColumnCommandHandler : IRequestHandler<UpdateColumnCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBoardNotificator _boardNotificator;

    public UpdateColumnCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBoardNotificator boardNotificator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _boardNotificator = boardNotificator;
    }

    public async Task<Result> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var column = await uow.ColumnRepository.GetAsync(request.Id);

        if (column is null)
        {
            return Result.Failure("Column was not found", ErrorType.NotFound);
        }

        column.Title = request.Title;
        column.Order = request.Order;
        column.UpdatedBy = request.UpdatedBy;
        column.UpdatedAt = DateTime.UtcNow;

        await uow.ColumnRepository.UpdateAsync(column);

        uow.Commit();

        await _boardNotificator.ColumnUpdatedAsync(request.BoardId, column.ToColumnSummaryDto());

        return Result.Success();
    }
}
