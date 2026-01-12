using MediatR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Users.Queries.SearchUsers;

public class SearchUsersCommandHandler : IRequestHandler<SearchUsersCommand, IEnumerable<UserSummaryDto>>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public SearchUsersCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<IEnumerable<UserSummaryDto>> Handle(SearchUsersCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var users = await uow.UserRepository.SearchByNameOrTag(request.SearchPrompt, request.PageSize);

        uow.Commit();

        return users.Select(u => u.ToUserSummaryDto()).ToList() ?? [];
    }
}
