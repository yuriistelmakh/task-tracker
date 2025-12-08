using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public UpdateUserCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var user = await uow.UserRepository.GetAsync(request.Id);

        if (user is null)
        {
            return false;
        }

        user.Tag = request.Tag;
        user.PasswordHash = request.PasswordHash;
        user.DisplayName = request.DisplayName;
        user.AvatarUrl = request.AvatarUrl;

        await uow.UserRepository.UpdateAsync(user);

        uow.Commit();

        return true;
    }
}
