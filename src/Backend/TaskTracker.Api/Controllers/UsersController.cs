using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Users.Commands.CreateUser;
using TaskTracker.Application.Features.Users.Commands.DeleteUser;
using TaskTracker.Application.Features.Users.Commands.UpdateUser;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
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
            Tag = request.Tag,
            AvatarUrl = request.AvatarUrl,
            DisplayName = request.DisplayName,
            PasswordHash = request.PasswordHash
        };

        var isSuccess = await _mediator.Send(command);

        return isSuccess
            ? NoContent()
            : Ok();
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
}
