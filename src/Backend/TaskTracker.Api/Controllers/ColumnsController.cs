using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Columns.Commands.CreateBoardColumns;
using TaskTracker.Application.Features.Columns.Commands.DeleteColumn;
using TaskTracker.Application.Features.Columns.Commands.UpdateColumn;
using TaskTracker.Domain.DTOs.Columns;

namespace TaskTracker.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ColumnsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ColumnsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateColumnRequest request)
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateColumnRequest request)
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
            : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
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
