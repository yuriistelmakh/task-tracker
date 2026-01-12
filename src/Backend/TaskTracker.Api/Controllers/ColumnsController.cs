using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Columns.Commands.CreateBoardColumns;
using TaskTracker.Application.Features.Columns.Commands.DeleteColumn;
using TaskTracker.Application.Features.Columns.Commands.ReorderColumnTasks;
using TaskTracker.Application.Features.Columns.Commands.UpdateColumn;
using TaskTracker.Domain.DTOs.Columns;

namespace TaskTracker.Api.Controllers;
[Route("api/[controller]")]
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
    public async Task<IActionResult> CreateAsync([FromBody] CreateColumnRequest request)
    {
        var command = new CreateColumnCommand
        {
            Title = request.Title,
            CreatedBy = request.CreatedBy,
            BoardId = request.BoardId,
            Order = request.Order
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("{id}/reorder")]
    public async Task<IActionResult> ReorderAsync(ReorderColumnTasksRequest request)
    {
        var command = new ReorderColumnTasksCommand
        {
            IdToOrder = request.MoveTaskRequests.ToDictionary(r => r.TaskId, r => r.NewOrder),
            ColumnId = request.ColumnId,
            TaskId = request.MovedTaskId
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : BadRequest();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateColumnRequest request)
    {
        var command = new UpdateColumnCommand
        {
            Id = id,
            Title = request.Title,
            Order = request.Order,
            UpdatedBy = request.UpdatedBy
        };

        var isSucess = await _mediator.Send(command);

        return isSucess
            ? NoContent()
            : BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var command = new DeleteColumnCommand
        {
            Id = id
        };

        var result = await _mediator.Send(command);

        return result
            ? NoContent()
            : NotFound();
    }
}
