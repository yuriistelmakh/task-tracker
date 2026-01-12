using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.BoardMembers.Queries.GetBoardMemberById;

public class GetBoardMemberByIdQueryHandler : IRequestHandler<GetBoardMemberByIdQuery?, Result<MemberSummaryDto>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetBoardMemberByIdQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result<MemberSummaryDto>> Handle(GetBoardMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var member = await uow.MemberRepository.GetByIdsAsync(request.BoardId, request.UserId);

        uow.Commit();

        return member is null
            ? Result<MemberSummaryDto>.NotFound("Member was not found")
            : Result<MemberSummaryDto>.Success(member.User.ToMemberSummaryDto(member.Role));
    }
}
