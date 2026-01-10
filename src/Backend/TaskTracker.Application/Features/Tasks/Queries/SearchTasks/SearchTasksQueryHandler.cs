using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Tasks.Queries.SearchTasks;

public class SearchTasksQueryHandler : IRequestHandler<SearchTasksQuery, Result<IEnumerable<TaskSummaryDto>>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public SearchTasksQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result<IEnumerable<TaskSummaryDto>>> Handle(SearchTasksQuery request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var tasks = await uow.TaskRepository.SearchAsync(request.BoardId, request.Prompt, request.PageSize);

        uow.Commit();

        var dtos = tasks.Select(t => t.ToTaskSummaryDto());

        return Result<IEnumerable<TaskSummaryDto>>.Success(dtos);
    }
}
