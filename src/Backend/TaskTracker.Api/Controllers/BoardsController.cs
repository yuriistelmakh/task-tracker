using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Boards.Commands.CreateBoard;
using TaskTracker.Application.Features.Boards.Commands.DeleteBoard;
using TaskTracker.Application.Features.Boards.Commands.ReorderBoardColumns;
using TaskTracker.Application.Features.Boards.Commands.UpdateBoard;
using TaskTracker.Application.Features.Boards.Queries.GetAllBoards;
using TaskTracker.Application.Features.Boards.Queries.GetBoardById;
using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("my-boards")]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page, [FromQuery] int pageSize)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Id was not found in the token");
        }

        int id = int.Parse(userIdString);

        var query = new GetAllBoardsQuery 
        { 
            UserId = id,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        return result is null 
            ? NotFound()
            : Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var query = new GetBoardByIdQuery(id);
        var result = await _mediator.Send(query);

        return result is null 
            ? NotFound()
            : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateBoardRequest request)
    {
        var command = new CreateBoardCommand
        {
            Title = request.Title,
            CreatedBy = request.CreatedBy,
            BackgroundColor = request.BackgroundColor,
            Visibility = request.Visibility
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateBoardRequest request)
    {
        var command = new UpdateBoardCommand
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            UpdatedBy = request.UpdatedBy,
            IsArchived = request.IsArchived,
            BackgroundColor = request.BackgroundColor
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var command = new DeleteBoardCommand
        {
            Id = id
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : NotFound();
    }

    [HttpPost("{id}/reorder")]
    public async Task<IActionResult> ReorderColumnsAsync(int id, [FromBody] ReorderBoardColumnsRequest request)
    {
        var command = new ReorderBoardColumnsCommand
        {
            IdToOrder = request.MoveColumnRequests.ToDictionary(r => r.ColumnId, r => r.NewOrder),
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : NotFound();
    }
}
