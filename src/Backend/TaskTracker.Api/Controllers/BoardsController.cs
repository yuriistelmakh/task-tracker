using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Boards.Commands.CreateBoard;
using TaskTracker.Application.Features.Boards.Queries.GetAllBoards;
using TaskTracker.Application.Features.Boards.Queries.GetBoardById;
using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // TODO: Refactor to get userId from UserClaims (from Auth)
    [HttpGet]
    public async Task<IActionResult> GetAll(int userId)
    {
        var command = new GetAllBoardsCommand()
        {
            UserId = userId
        };

        var result = await _mediator.Send(command);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetBoardByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBoardRequest request)
    {
        var command = new CreateBoardCommand
        {
            Title = request.Title,
            Description = request.Description,
            CreatedBy = request.CreatedBy
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
