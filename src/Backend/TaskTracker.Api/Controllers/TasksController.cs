using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Tasks.Commands.CreateTask;
using TaskTracker.Application.Features.Tasks.Commands.DeleteTask;
using TaskTracker.Application.Features.Tasks.Commands.UpdateTask;
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
    public async Task<IActionResult> CreateAsync([FromBody] CreateTaskRequest request)
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateTaskRequest request)
    {
        var command = new UpdateTaskCommand
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            AssigneeId = request.AssigneeId,
            IsComplete = request.IsComplete,
            Order = request.Order,
            ColumnId = request.ColumnId,
            DueDate = request.DueDate,
            Priority = request.Priority,
            UpdatedBy = request.UpdatedBy
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var command = new DeleteTaskCommand
        {
            Id = id
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess 
            ? NoContent()
            : NotFound();
    }
}
