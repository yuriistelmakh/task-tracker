using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Boards.Commands.UpdateBoardMemberRole;

public class UpdateBoardMemberRoleCommandHandler : IRequestHandler<UpdateBoardMemberRoleCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public UpdateBoardMemberRoleCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(UpdateBoardMemberRoleCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var member = await uow.MemberRepository.GetByIdsAsync(request.BoardId, request.UserId);

        if (member is null)
        {
            return false;
        }

        member.Role = request.NewRole;

        var result = await uow.MemberRepository.UpdateAsync(member);

        uow.Commit();

        return true;
    }
}
