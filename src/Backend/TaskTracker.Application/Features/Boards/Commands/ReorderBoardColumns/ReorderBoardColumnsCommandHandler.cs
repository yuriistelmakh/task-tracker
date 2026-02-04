using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Boards.Commands.ReorderBoardColumns;

public class ReorderBoardColumnsCommandHandler : IRequestHandler<ReorderBoardColumnsCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBoardNotificator _boardNotificator;

    public ReorderBoardColumnsCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBoardNotificator boardNotificator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _boardNotificator = boardNotificator;
    }

    public async Task<bool> Handle(ReorderBoardColumnsCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        foreach (var kvp in request.IdToOrder)
        {
            var isSuccessful = await uow.ColumnRepository.UpdateOrderAsync(kvp.Key, kvp.Value);

            if (!isSuccessful)
            {
                return false;
            }
        }

        uow.Commit();

        await _boardNotificator.BoardChangedAsync(request.BoardId);

        return true;
    }
}
