using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Tasks.Commands.ChangeStatus;
using TaskTracker.Application.Features.Tasks.Commands.CreateTask;
using TaskTracker.Application.Features.Tasks.Commands.DeleteTask;
using TaskTracker.Application.Features.Tasks.Commands.UpdateTask;
using TaskTracker.Application.Features.Tasks.Queries.GetTaskById;
using TaskTracker.Application.Features.Tasks.Queries.SearchTasks;
using TaskTracker.Domain.DTOs.Tasks;

namespace TaskTracker.Api.Controllers;

[Route("api/boards/{boardId:int}/tasks")]
[ApiController]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int boardId, [FromRoute] int id)
    {
        var query = new GetTaskByIdQuery
        {
            Id = id
        };

        var result = await _mediator.Send(query);

        return result is null
            ? NotFound()
            : Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync(
        [FromRoute] int boardId,
        [FromQuery] string? prompt,
        [FromQuery] int page,
        [FromQuery] int pageSize
        )
    {
        var query = new SearchTasksQuery
        {
            BoardId = boardId,
            Prompt = prompt,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromRoute] int boardId, [FromBody] CreateTaskRequest request)
    {
        var command = new CreateTaskCommand
        {
            ColumnId = request.ColumnId,
            Title = request.Title,
            CreatedBy = request.CreatedBy,
            Order = request.Order,
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int boardId, [FromRoute] int id, [FromBody] UpdateTaskRequest request)
    {
        var command = new UpdateTaskCommand
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            AssigneeId = request.AssigneeId,
            IsComplete = request.IsComplete,
            DueDate = request.DueDate,
            Priority = request.Priority,
            UpdatedBy = request.UpdatedBy
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : NotFound();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus([FromRoute] int boardId, [FromRoute] int id, [FromBody] ChangeTaskStatusRequest request)
    {
        var command = new ChangeTaskStatusCommand
        {
            Id = id,
            IsComplete = request.IsComplete,
            UpdatedBy = request.UpdatedBy
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
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
