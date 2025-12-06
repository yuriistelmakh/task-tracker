using MediatR;
using TaskTracker.Domain.DTOs.Auth;

namespace TaskTracker.Application.Features.Auth.Commands.TokenRefresh;

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public required string RefreshToken { get; set; }

    public required string AccessToken { get; set; }
}
