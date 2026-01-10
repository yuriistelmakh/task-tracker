using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Auth.Commands.Login;
using TaskTracker.Application.Features.Auth.Commands.TokenRefresh;
using TaskTracker.Application.Features.Auth.Commands.Signup;
using TaskTracker.Domain.DTOs.Auth;

namespace TaskTracker.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password,
            Tag = request.Tag
        };

        var result = await _mediator.Send(command);
        
        return Ok(result);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignupAsync([FromBody] SignupRequest request)
    {
        var command = new SignupCommand
        {
            Email = request.Email,
            Password = request.Password,
            DisplayName = request.DisplayName,
            Tag = request.Tag
        };

        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshRequest request)
    {
        var command = new RefreshTokenCommand
        {
            AccessToken = request.AccessToken,
            RefreshToken = request.RefreshToken
        };

        var result = await _mediator.Send(command);

        return result is null
            ? BadRequest()
            : Ok(result);
    }
}
