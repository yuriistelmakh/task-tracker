using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.SignalR;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Columns.Commands.DeleteColumn;

public class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IBoardNotificator _boardNotificator;

    public DeleteColumnCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IBoardNotificator boardNotificator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _boardNotificator = boardNotificator;
    }

    public async Task<Result> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        await uow.ColumnRepository.DeleteAsync(request.Id);

        uow.Commit();

        await _boardNotificator.ColumnDeletedAsync(request.BoardId, request.Id);

        return Result.Success();
    }
}
