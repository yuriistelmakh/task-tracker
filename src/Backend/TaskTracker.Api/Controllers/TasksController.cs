using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Tasks.Commands;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var command = new CreateTaskCommand
        {
            ColumnId = request.ColumnId,
            Title = request.Title,
            CreatedBy = request.CreatedBy,
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
