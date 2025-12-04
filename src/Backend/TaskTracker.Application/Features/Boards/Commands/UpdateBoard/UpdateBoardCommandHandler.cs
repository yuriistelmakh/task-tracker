using MediatR;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

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

        var board = await uow.BoardRepository.GetByIdDetailsAsync(request.Id);

        if (board is null)
        {
            return false;
        }

        board.Title = request.Title;
        board.Description = request.Description;
        board.UpdatedBy = request.UpdatedBy;
        board.UpdatedAt = DateTime.UtcNow;

        var rowsAffected = await uow.BoardRepository.UpdateAsync(board);

        uow.Commit();

        return rowsAffected > 0;
    }
}
