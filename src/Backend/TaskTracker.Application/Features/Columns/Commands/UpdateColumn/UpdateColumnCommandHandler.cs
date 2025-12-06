using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Columns.Commands.UpdateColumn;

public class UpdateColumnCommandHandler : IRequestHandler<UpdateColumnCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public UpdateColumnCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var column = await uow.ColumnRepository.GetAsync(request.Id);

        if (column is null)
        {
            return false;
        }

        column.Title = request.Title;
        column.Order = request.Order;
        column.UpdatedBy = request.UpdatedBy;
        column.UpdatedAt = DateTime.UtcNow;

        await uow.ColumnRepository.UpdateAsync(column);

        uow.Commit();

        return true;
    }
}
