using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.DTOs.Users;
using TaskTracker.Domain.Mapping;

namespace TaskTracker.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse?>
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

    public async Task<AuthResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        var user = await uow.UserRepository.GetByEmailOrTagAsync(request.Email, request.Tag);

        if (user is null)
        {
            return null;
        }

        bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return null;
        }

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        refreshToken.UserId = user.Id;

        await uow.RefreshTokenRepository.AddAsync(refreshToken);

        uow.Commit();

        return new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken.Token };
    }
}
