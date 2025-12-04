using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Boards.Commands.AddNewMember;
using TaskTracker.Application.Features.Boards.Commands.CreateBoard;
using TaskTracker.Application.Features.Boards.Commands.DeleteBoard;
using TaskTracker.Application.Features.Boards.Commands.UpdateBoard;
using TaskTracker.Application.Features.Boards.Queries.GetAllBoards;
using TaskTracker.Application.Features.Boards.Queries.GetAllMembers;
using TaskTracker.Application.Features.Boards.Queries.GetBoardById;
using TaskTracker.Domain.DTOs.BoardMember;
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
        var query = new GetAllBoardsQuery(userId);

        var result = await _mediator.Send(query);

        return result is null 
            ? NotFound()
            : Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetBoardByIdQuery(id);
        var result = await _mediator.Send(query);

        return result is null 
            ? NotFound()
            : Ok(result);
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

    [HttpGet("{boardId}/members")]
    public async Task<IActionResult> GetMembers(int boardId)
    {
        var query = new GetBoardMembersQuery(boardId);

        var result = await _mediator.Send(query);

        return result is null 
            ? NotFound()
            : Ok(result);
    }

    [HttpPost("{boardId}/members")]
    public async Task<IActionResult> AddMember(int boardId, [FromBody] AddBoardMemberRequest request)
    {
        var command = new AddNewMemberCommand
        {
            UserId = request.UserId,
            BoardId = boardId,
            Role = request.Role
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBoardRequest request)
    {
        var command = new UpdateBoardCommand
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            UpdatedBy = request.UpdatedBy,
            IsArchived = request.IsArchived
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
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
}
