using MediatR;
using System.Collections;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs;
using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.Application.Features.Boards.Queries.GetAllBoards;

public class GetAllBoardsQuery : IRequest<PagedResponse<BoardSummaryDto>>
{
    public int UserId { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }
}
