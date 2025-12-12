using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Auth;

namespace TaskTracker.Application.Features.Auth.Commands.TokenRefresh;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResult?>
{
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _jwtTokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);
    }
}
