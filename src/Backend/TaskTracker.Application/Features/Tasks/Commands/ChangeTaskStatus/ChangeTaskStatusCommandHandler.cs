using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Tasks.Commands.ChangeStatus;

public class ChangeTaskStatusCommandHandler : IRequestHandler<ChangeTaskStatusCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public ChangeTaskStatusCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var result = await uow.TaskRepository.UpdateStatusAsync(request.Id, request.IsComplete, request.UpdatedBy);

        uow.Commit();

        return result;
    }
}
