using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading.Tasks;
using TaskTracker.Application.Features.Attachments.Commands.CreateAttachment;
using TaskTracker.Application.Features.Attachments.Commands.DeleteAttachment;
using TaskTracker.Application.Features.Attachments.Commands.RenameAttachment;
using TaskTracker.Application.Features.Attachments.Queries.GetAllAttachments;
using TaskTracker.Domain.DTOs.Attachments;

namespace TaskTracker.Api.Controllers;

[ApiController]
[Route("api/boards/{boardId:int}/tasks/{taskId:int}/attachments")]
public class AttachmentsController : Controller
{
    private readonly IMediator _mediator;

    public AttachmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] int boardId,
        [FromRoute] int taskId,
        [FromForm] int createdBy,
        IFormFile file)
    {
        var command = new CreateAttachmentCommand
        {
            TaskId = taskId,
            CreatedBy = createdBy,
            File = file
        };

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok()
            : BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromRoute] int boardId,
        [FromRoute] int taskId)
    {
        var query = new GetAllAttachmentsQuery
        {
            TaskId = taskId
        };

        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest();
    }

    [HttpDelete("{attachmentId}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int boardId,
        [FromRoute] int taskId,
        [FromRoute] int attachmentId)
    {
        var command = new DeleteAttachmentCommand
        {
            AttachmentId = attachmentId
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok()
            : BadRequest();
    }

    [HttpPatch("{attachmentId}/rename")]
    public async Task<IActionResult> RenameAsync(
        [FromRoute] int attachmentId,
        [FromBody] RenameAttachmentRequest request)
    {
        var command = new RenameAttachmentCommand
        {
            Id = attachmentId,
            NewName = request.NewName
        };

        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest();
    }
}
