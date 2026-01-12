using MediatR;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Application.Features.Users.Queries.SearchUsers;

public class SearchUsersCommand : IRequest<IEnumerable<UserSummaryDto>>
{
    public string? SearchPrompt { get; set; }

    public int PageSize { get; set; }
}
