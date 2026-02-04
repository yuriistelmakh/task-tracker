using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Application;
using TaskTracker.Application.Features.Users.Commands.CreateUser;
using TaskTracker.Application.Features.Users.Commands.DeleteUser;
using TaskTracker.Application.Features.Users.Commands.UpdateUser;
using TaskTracker.Application.Features.Users.Queries.GetNotifications;
using TaskTracker.Application.Features.Users.Queries.GetUserById;
using TaskTracker.Application.Features.Users.Queries.SearchUsers;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var query = new GetUserByIdQuery
        {
            Id = id
        };

        var result = await _mediator.Send(query);
        
        return result is null
            ? NotFound()
            : Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> SearchAsync(string? prompt, int pageSize)
    {
        var command = new SearchUsersCommand
        {
            SearchPrompt = prompt,
            PageSize = pageSize
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand
        {
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            DisplayName = request.DisplayName,
            Tag = request.Tag
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateUserRequest request)
    {
        var command = new UpdateUserCommand
        {
            Id = id,
            Email = request.Email,
            Tag = request.Tag,
            DisplayName = request.DisplayName
        };

        var result = await _mediator.Send(command);

        return result.ErrorType switch
        {
            ErrorType.None => Ok(),
            ErrorType.NotFound => NotFound(),
            ErrorType.Conflict => Conflict(result.ErrorMessage),
            _ => BadRequest(result.ErrorMessage)
        };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var command = new DeleteUserCommand
        {
            Id = id
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : NotFound();
    }

    [HttpGet("{id}/notifications")]
    public async Task<IActionResult> GetUnreadNotificationsAsync(int id)
    {
        var query = new GetUserNotificationsQuery
        {
            UserId = id
        };

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
