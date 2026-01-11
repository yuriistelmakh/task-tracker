using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.BoardMembers.Queries.SearchBoardMembers;

public class SearchBoardMembersQueryHandler : IRequestHandler<SearchBoardMembersQuery, Result<PagedResponse<MemberSummaryDto>>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public SearchBoardMembersQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result<PagedResponse<MemberSummaryDto>>> Handle(SearchBoardMembersQuery request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var result = await uow.MemberRepository.SearchByNameOrTag(request.BoardId, request.Prompt, request.PageSize, request.Page);
        uow.Commit();

        var dtos = result.Items.Select(m => m.ToMemberSummaryDto(m.Role));

        var response = new PagedResponse<MemberSummaryDto>
        {
            Items = dtos ?? [],
            TotalCount = result.Count
        };

        return Result<PagedResponse<MemberSummaryDto>>.Success(response);
    }
}
