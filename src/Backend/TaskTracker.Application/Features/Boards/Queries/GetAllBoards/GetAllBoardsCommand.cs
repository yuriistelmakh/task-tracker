using MediatR;
using System.Collections;
using System.Collections.Generic;
using TaskTracker.Domain.DTOs.Boards;

namespace TaskTracker.Application.Features.Boards.Queries.GetAllBoards;

public class GetAllBoardsCommand : IRequest<IEnumerable<BoardSummaryDto>>
{
    public int UserId { get; set; }
}
