using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;
using TaskTracker.Domain.DTOs;
using TaskTracker.Domain.DTOs.Boards;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Domain.DTOs.Users;

namespace TaskTracker.Application.Features.Boards.Queries.GetBoardById;

public class GetBoardByIdQueryHandler : IRequestHandler<GetBoardByIdQuery, BoardDetailsDto?>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public GetBoardByIdQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<BoardDetailsDto?> Handle(GetBoardByIdQuery query, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();
        var board = await uow.BoardRepository.GetByIdDetailsAsync(query.Id);

        if (board is null)
        {
            return null;
        }

        uow.Commit();

        return new BoardDetailsDto
        {
            Id = board.Id,
            Title = board.Title,
            Description = board.Description,
            IsArchived = board.IsArchived,
            CreatedAt = board.CreatedAt,
            Owner = new UserSummaryDto
            {
                Id = board.Creator.Id,
                DisplayName = board.Creator.DisplayName,
                Tag = board.Creator.Tag,
                AvatarUrl = board.Creator.AvatarUrl
            },
            Columns = board.Columns?.Select(c => new ColumnSummaryDto
            {
                Id = c.Id,
                Title = c.Title,
                Order = c.Order,
                Tasks = c.Tasks?.Select(t => new TaskSummaryDto()
                {
                   Id = t.Id,
                   Title = t.Title,
                   AssigneeId = t.AssigneeId,
                   ColumndId = t.ColumnId,
                   Order = t.Order,
                   Priority = t.Priority
                }).ToList() ?? []
            }).ToList() ?? []
        };
    }
}
