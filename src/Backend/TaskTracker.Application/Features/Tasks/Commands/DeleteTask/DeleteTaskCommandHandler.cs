using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;

namespace TaskTracker.Application.Features.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    
    public DeleteTaskCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var rowsAffected = await uow.TaskRepository.DeleteAsync(request.Id);

        uow.Commit();

        return rowsAffected > 0;
    }
}
