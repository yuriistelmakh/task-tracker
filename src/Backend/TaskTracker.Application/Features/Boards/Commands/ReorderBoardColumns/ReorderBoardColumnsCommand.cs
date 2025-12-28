using MediatR;
using System.Collections.Generic;

namespace TaskTracker.Application.Features.Boards.Commands.ReorderBoardColumns;

public class ReorderBoardColumnsCommand : IRequest<bool>
{
    public Dictionary<int, int> IdToOrder { get; set; } = [];
}
