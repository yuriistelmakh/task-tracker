using MediatR;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public UpdateUserCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var userWithSameTag = await uow.UserRepository.GetByTagAsync(request.Tag);

        if (userWithSameTag is not null && userWithSameTag.Id != request.Id)
        {
            return Result.Conflict($"User with that tag already exists");
        }

        var userWithSameEmail = await uow.UserRepository.GetByEmailAsync(request.Email);

        if (userWithSameEmail is not null && userWithSameEmail.Id != request.Id)
        {
            return Result.Conflict($"User with that email already exists");
        }

        var user = await uow.UserRepository.GetAsync(request.Id);

        if (user is null)
        {
            return Result.NotFound($"User with id {request.Id} was not found");
        }

        user.Email = request.Email;
        user.Tag = request.Tag;
        user.DisplayName = request.DisplayName;

        await uow.UserRepository.UpdateAsync(user);

        uow.Commit();

        return Result.Success();
    }
}
