using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.BoardMembers.Commands.KickMember;

public class KickMemberCommandHandler : IRequestHandler<KickMemberCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public KickMemberCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result> Handle(KickMemberCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var member = await uow.MemberRepository.GetByIdsAsync(request.BoardId, request.UserId);

        if (member is null)
        {
            return Result.Failure("Member not found", ErrorType.NotFound);
        }

        var rowsAffected = await uow.MemberRepository.DeleteAsync(member.Id);

        var tasksWithDeletedMember = await uow.TaskRepository.GetAllByAssignee(request.UserId);

        foreach (var task in tasksWithDeletedMember)
        {
            task.AssigneeId = null;
            await uow.TaskRepository.UpdateAsync(task);
        }

        uow.Commit();

        if (rowsAffected == 0)
        {
            return Result.Failure("Failed to remove member", ErrorType.Failure);
        }
        
        return Result.Success();
    }
}
