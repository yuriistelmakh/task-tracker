using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Features.Boards.Commands.CreateBoard;
using TaskTracker.Application.Features.Boards.Queries.GetBoardById;

namespace TaskTracker.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBoardCommand command)
    {
        var boardId = await _mediator.Send(command);

        return Ok(boardId);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetBoardByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
