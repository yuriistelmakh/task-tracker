using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
[Authorize]
public class BoardsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoardsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("my-boards")]
    public async Task<IActionResult> GetAllAsync()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Id was not found in the token");
        }

        int id = int.Parse(userIdString);

        var query = new GetAllBoardsQuery(id);

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
            Description = request.Description,
            CreatedBy = request.CreatedBy
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("{boardId}/members")]
    public async Task<IActionResult> GetMembersAsync(int boardId)
    {
        var query = new GetBoardMembersQuery(boardId);

        var result = await _mediator.Send(query);

        return result is null 
            ? NotFound()
            : Ok(result);
    }

    [HttpPost("{boardId}/members")]
    public async Task<IActionResult> AddMemberAsync(int boardId, [FromBody] AddBoardMemberRequest request)
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
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateBoardRequest request)
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
}
