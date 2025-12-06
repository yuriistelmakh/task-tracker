using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.Application.Features.Boards.Commands.DeleteBoard;

public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public DeleteBoardCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var rowsDeleted = await uow.BoardRepository.DeleteAsync(request.Id);

        uow.Commit();

        return rowsDeleted > 0;
    }
}
