using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.BoardMembers.Commands.AddNewMember;

public class AddNewMemberCommandHandler : IRequestHandler<AddNewMemberCommand, int>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public AddNewMemberCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<int> Handle(AddNewMemberCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var boardMember = new BoardMember
        {
            UserId = request.UserId,
            BoardId = request.BoardId,
            JoinedAt = DateTime.UtcNow,
            Role = request.Role
        };

        var result = await uow.MemberRepository.AddAsync(boardMember);

        uow.Commit();

        return result;
    }
}
