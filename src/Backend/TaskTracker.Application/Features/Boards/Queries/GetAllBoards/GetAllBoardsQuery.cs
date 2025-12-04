using MediatR;
using System.Collections;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.Application.Features.Boards.Queries.GetAllBoards;

public class GetAllBoardsQuery : IRequest<IEnumerable<BoardSummaryDto>>
{
    public int UserId { get; set; }

    public GetAllBoardsQuery(int userId)
    {
        UserId = userId;
    }
}
