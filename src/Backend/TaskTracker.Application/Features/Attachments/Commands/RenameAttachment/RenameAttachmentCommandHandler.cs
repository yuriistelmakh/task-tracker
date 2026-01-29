using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Attachments.Commands.RenameAttachment;

public class RenameAttachmentCommandHandler : IRequestHandler<RenameAttachmentCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public RenameAttachmentCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result> Handle(RenameAttachmentCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var attachment = await uow.AttachmentRepository.GetAsync(request.Id);

        if (attachment is null)
        {
            return Result.NotFound($"Attachment with id {request.Id} not found.");
        }

        attachment.Name = request.NewName;

        await uow.AttachmentRepository.UpdateAsync(attachment);

        uow.Commit();

        return Result.Success();
    }
}
