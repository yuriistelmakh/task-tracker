using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Columns.Commands.CreateBoardColumns;
using TaskTracker.Application.Features.Columns.Commands.DeleteColumn;
using TaskTracker.Application.Features.Columns.Commands.ReorderColumnTasks;
using TaskTracker.Application.Features.Columns.Commands.UpdateColumn;
using TaskTracker.Domain.DTOs.Columns;

namespace TaskTracker.Api.Controllers;
[Route("api/boards/{boardId}/columns")]
[ApiController]
[Authorize]
public class ColumnsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ColumnsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromRoute] int boardId, [FromBody] CreateColumnRequest request)
    {
        var command = new CreateColumnCommand
        {
            Title = request.Title,
            CreatedBy = request.CreatedBy,
            BoardId = request.BoardId,
            Order = request.Order
        };

        var result = await _mediator.Send(command);

        return Ok(result.Value);
    }

    [HttpPost("{columnId}/reorder")]
    public async Task<IActionResult> ReorderAsync(
        [FromRoute] int boardId,
        [FromRoute] int columnId,
        [FromBody] ReorderColumnTasksRequest request)
    {
        var command = new ReorderColumnTasksCommand
        {
            IdToOrder = request.MoveTaskRequests.ToDictionary(r => r.TaskId, r => r.NewOrder),
            ColumnId = columnId,
            BoardId = boardId,
            TaskId = request.MovedTaskId
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : BadRequest();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] int boardId, 
        [FromRoute] int id, 
        [FromBody] UpdateColumnRequest request)
    {
        var command = new UpdateColumnCommand
        {
            Id = id,
            BoardId = boardId,
            Title = request.Title,
            Order = request.Order,
            UpdatedBy = request.UpdatedBy
        };

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? NoContent()
            : BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int boardId, [FromRoute] int id)
    {
        var command = new DeleteColumnCommand
        {
            Id = id,
            BoardId = boardId
        };

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? NoContent()
            : NotFound();
    }
}
