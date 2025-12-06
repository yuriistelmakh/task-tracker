using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Columns.Commands.DeleteColumn;

public class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public DeleteColumnCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var rowsDeleted = await uow.ColumnRepository.DeleteAsync(request.Id);

        uow.Commit();

        return rowsDeleted > 0;
    }
}
