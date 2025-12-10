using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Auth.Commands.Signup;

public class SignupCommandHandler : IRequestHandler<SignupCommand, AuthResponse>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenGenerator;

    public SignupCommandHandler(IUnitOfWorkFactory unitOfWorkFactory,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenGenerator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        if (await uow.UserRepository.GetByEmailAsync(request.Email) is not null)
        {
            return new AuthResponse { ErrorType = AuthErrorType.EmailTaken };
        }

        if (await uow.UserRepository.GetByTagAsync(request.Tag) is not null)
        {
            return new AuthResponse { ErrorType = AuthErrorType.TagTaken };
        }
        
        var passwordHash = _passwordHasher.Generate(request.Password);

        var user = new User
        {
            Email = request.Email,
            DisplayName = request.DisplayName,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            Role = Roles.User,
            Tag = request.Tag
        };

        var id = await uow.UserRepository.AddAsync(user);
        user.Id = id;

        string accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        RefreshToken refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        refreshToken.UserId = user.Id;

        await uow.RefreshTokenRepository.AddAsync(refreshToken);

        uow.Commit();

        return new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken.Token };
    }
}
