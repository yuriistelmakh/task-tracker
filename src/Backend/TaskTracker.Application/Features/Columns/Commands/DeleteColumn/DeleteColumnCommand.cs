using MediatR;

namespace TaskTracker.Application.Features.Columns.Commands.DeleteColumn;

public class DeleteColumnCommand : IRequest<bool>
{
    public int Id { get; set; }
}
