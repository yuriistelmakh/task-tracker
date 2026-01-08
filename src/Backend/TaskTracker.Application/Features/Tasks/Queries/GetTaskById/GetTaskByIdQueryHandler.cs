using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDetailsDto?>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetTaskByIdQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<TaskDetailsDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var task = await uow.TaskRepository.GetAsync(request.Id);

        if (task is null)
        {
            return null;
        }

        var dto = task.ToTaskDetailsDto();

        var column = await uow.ColumnRepository.GetAsync(task.ColumnId);

        if (column is null)
        {
            return null;
        }

        dto.ColumnTitle = column.Title;

        if (task.AssigneeId.HasValue)
        {
            var user = await uow.UserRepository.GetAsync(task.AssigneeId.Value);

            if (user is null)
            {
                return null;
            }

            dto.AssigneeDto = user.ToUserSummaryDto();
        }

        uow.Commit();

        return dto;
    }
}
