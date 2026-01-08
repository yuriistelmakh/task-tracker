using MediatR;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Application.Features.Tasks.Queries.GetTaskById;

public class GetTaskByIdQuery : IRequest<TaskDetailsDto?>
{
    public int Id { get; set; }
}
