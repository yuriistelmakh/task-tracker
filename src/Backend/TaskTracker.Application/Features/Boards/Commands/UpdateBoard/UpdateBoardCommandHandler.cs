using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Boards.Commands.UpdateBoard;

public class UpdateBoardCommandHandler : IRequestHandler<UpdateBoardCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public UpdateBoardCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var board = await uow.BoardRepository.GetDetailsAsync(request.Id);

        if (board is null)
        {
            return false;
        }

        board.Title = request.Title;
        board.Description = request.Description;
        board.UpdatedBy = request.UpdatedBy;
        board.UpdatedAt = DateTime.UtcNow;
        board.IsArchived = request.IsArchived;
        board.BackgroundColor = request.BackgroundColor;

        await uow.BoardRepository.UpdateAsync(board);

        uow.Commit();

        return true;
    }
}
