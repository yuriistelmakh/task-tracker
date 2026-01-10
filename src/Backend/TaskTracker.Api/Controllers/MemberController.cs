using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Application;
using TaskTracker.Application.Features.BoardMembers.Commands.AcceptInvitation;
using TaskTracker.Application.Features.BoardMembers.Commands.AddNewMember;
using TaskTracker.Application.Features.BoardMembers.Commands.RejectInvitation;
using TaskTracker.Application.Features.BoardMembers.Commands.SendInvitations;
using TaskTracker.Application.Features.BoardMembers.Commands.UpdateBoardMemberRole;
using TaskTracker.Application.Features.BoardMembers.Queries.GetBoardMemberById;
using TaskTracker.Application.Features.BoardMembers.Queries.GetBoardMembers;
using TaskTracker.Application.Features.BoardMembers.Queries.SearchBoardMembers;
using TaskTracker.Domain.DTOs.BoardMember;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/boards/{boardId:int}/members")]
public class BoardMembersController : Controller
{
    private readonly IMediator _mediator;

    public BoardMembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromRoute] int boardId, 
        [FromQuery] int? page = null, 
        [FromQuery] int? pageSize = null)
    {
        var query = new GetBoardMembersQuery
        {
            BoardId = boardId,
            Page = page,
            PageSize = pageSize
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
        var query = new SearchBoardMembersQuery
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

    [HttpGet("{userid:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int boardId, [FromRoute] int userId)
    {
        var query = new GetBoardMemberByIdQuery
        {
            BoardId = boardId,
            UserId = userId
        };

        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromRoute] int boardId, [FromBody] AddBoardMemberRequest request)
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

    [HttpPut("{userId:int}")]
    public async Task<IActionResult> UpdateRoleAsync(
        [FromRoute] int boardId, 
        [FromRoute] int userId, 
        [FromBody] UpdateBoardMemberRoleRequest request)
    {
        var command = new UpdateBoardMemberRoleCommand
        {
            BoardId = boardId,
            UserId = userId,
            NewRole = request.NewRole,
        };

        var isSuccessful = await _mediator.Send(command);

        return isSuccessful
            ? NoContent()
            : NotFound();
    }

    [HttpPost("invitations")]
    public async Task<IActionResult> SendInvitationAsync([FromRoute] int boardId, [FromBody] SendInvitationRequest request)
    {
        var command = new SendInvitationCommand
        {
            InviteeId = request.InviteeId,
            InviterId = request.InviterId,
            BoardId = boardId,
            Role = request.Role
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return NoContent();
        }
        else
        {
            return result.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result.ErrorMessage),
                ErrorType.Conflict => Conflict(result.ErrorMessage),
                _ => StatusCode(500, result.ErrorMessage)
            };
        }
    }

    [HttpPost("invitations/accept")]
    public async Task<IActionResult> AcceptInvitationAsync([FromRoute] int boardId, [FromBody] AcceptInvitationRequest request)
    {
        var command = new AcceptInvitationCommand
        {
            BoardId = boardId,
            InvitationId = request.InvitationId,
            InviteeId = request.InviteeId,
            NotificationId = request.NotificationId,
            Role = request.Role
        };

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? NoContent()
            : NotFound();
    }

    [HttpPost("invitations/reject")]
    public async Task<IActionResult> RejectInvitationAsync([FromRoute] int boardId, [FromBody] RejectInvitationRequest request)
    {
        var command = new RejectInvitationCommand
        {
            InvitationId = request.InvitationId,
            NotificationId = request.NotificationId
        };

        var isSuccessful = await _mediator.Send(command);

        return isSuccessful
            ? NoContent()
            : NotFound();
    }
}
