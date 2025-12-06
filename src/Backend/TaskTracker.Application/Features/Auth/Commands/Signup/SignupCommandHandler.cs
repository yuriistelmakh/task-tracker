using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Application.Features.Auth.Commands.Signup;

public class SignupCommandHandler : IRequestHandler<SignupCommand, string?>
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public SignupCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, 
        IPasswordHasher passwordHasher, 
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string?> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        using var uow = _unitOfWorkFactory.Create();

        if (await uow.UserRepository.GetByEmailOrTagAsync(request.Email, request.Tag) is not null)
        {
            return null;
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

        await uow.UserRepository.AddAsync(user);

        uow.Commit();

        return _jwtTokenGenerator.GenerateToken(user);
    }
}
