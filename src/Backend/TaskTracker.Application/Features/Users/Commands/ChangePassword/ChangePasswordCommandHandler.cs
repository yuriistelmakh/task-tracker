using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Interfaces.UoW;

namespace TaskTracker.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IPasswordHasher passwordHasher)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var uow = _unitOfWorkFactory.Create();

        var user = await uow.UserRepository.GetAsync(request.UserId);

        if (user is null)
        {
            return Result.NotFound($"User with id {request.UserId} was not found");
        }

        if (!_passwordHasher.Verify(request.OldPassword, user.PasswordHash))
        {
            return Result.Conflict("Old password is incorrect");
        }

        user.PasswordHash = _passwordHasher.Generate(request.NewPassword);

        await uow.UserRepository.UpdateAsync(user);

        uow.Commit();

        return Result.Success();
    }
}
