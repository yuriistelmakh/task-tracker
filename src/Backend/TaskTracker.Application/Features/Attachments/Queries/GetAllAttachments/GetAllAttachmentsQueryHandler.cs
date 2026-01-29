using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Attachments;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Attachments.Queries.GetAllAttachments;

public class GetAllAttachmentsQueryHandler : IRequestHandler<GetAllAttachmentsQuery, Result<IEnumerable<AttachmentDto>>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetAllAttachmentsQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result<IEnumerable<AttachmentDto>>> Handle(GetAllAttachmentsQuery request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var attachments = await uow.AttachmentRepository.GetAllByTaskId(request.TaskId);

        uow.Commit();

        return Result<IEnumerable<AttachmentDto>>.Success(attachments.Select(a => a.ToAttachmentDto()));
    }
}
