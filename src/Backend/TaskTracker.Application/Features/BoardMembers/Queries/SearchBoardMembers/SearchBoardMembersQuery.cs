using MediatR;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Application.Features.BoardMembers.Queries.SearchBoardMembers;

public class SearchBoardMembersQuery : IRequest<Result<PagedResponse<MemberSummaryDto>>>
{
    public int BoardId { get; set; }

    public string? Prompt { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }
}