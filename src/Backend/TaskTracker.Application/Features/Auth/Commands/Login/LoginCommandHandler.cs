using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IJwtTokenService _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, 
        IPasswordHasher passwordHasher, 
        IJwtTokenService jwtTokenGenerator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var user = request.Email is not null
            ? await uow.UserRepository.GetByEmailAsync(request.Email)
            : await uow.UserRepository.GetByTagAsync(request.Tag);

        if (user is null)
        {
            return new AuthResult { ErrorType = AuthErrorType.UserNotFound };
        }

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return new AuthResult { ErrorType = AuthErrorType.InvalidPassword };
        }

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        refreshToken.UserId = user.Id;

        await uow.RefreshTokenRepository.AddAsync(refreshToken);

        uow.Commit();

        return new AuthResult
        {
            ErrorType = AuthErrorType.None,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
