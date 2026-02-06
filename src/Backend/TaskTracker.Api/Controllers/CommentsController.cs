using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Comments.Commands.CreateCommand;
using TaskTracker.Application.Features.Comments.Queries.GetComments;
using TaskTracker.Domain.DTOs.Comments;

namespace TaskTracker.Api.Controllers;

[Route("api/boards/{boardId:int}/tasks/{taskId:int}/comments")]
[ApiController]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(
        [FromRoute] int boardId,
        [FromRoute] int taskId,
        [FromQuery] int page,
        [FromQuery] int pageSize
    )
    {
        var command = new GetCommentsQuery
        {
            TaskId = taskId,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.ErrorMessage);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] int boardId,
        [FromRoute] int taskId,
        [FromBody] CreateCommentRequest request
        )
    {
        var command = new CreateCommentCommand
        {
            TaskId = taskId,
            CreatedBy = request.CreatedBy,
            Content = request.Content
        };

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.ErrorMessage);
    }
}
