using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public CreateUserCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();
        var user = new User
        {
            Email = command.Email,
            PasswordHash = command.PasswordHash,
            Tag = command.Tag,
            DisplayName = command.DisplayName,
            CreatedAt = DateTime.UtcNow
        };

        var result = await uow.UserRepository.AddAsync(user); 
        uow.Commit();

        return result;
    }
}
