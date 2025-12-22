using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.Interfaces.Auth;
using TaskTracker.Application.Interfaces.UoW;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;

namespace TaskTracker.Infrastructure.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public JwtTokenService(IConfiguration config, IUnitOfWorkFactory unitOfWorkFactory)
    {
        _config = config;
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    public string GenerateAccessToken(User user)
    {
        var secretKey = _config["JwtSettings:SecretKey"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.DisplayName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["JwtSettings:ExpiryMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal is null)
        {
            return new AuthResponse { ErrorType = AuthErrorType.Unknown };
        }

        var userIdString = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var userId = int.Parse(userIdString);

        using var uow = _unitOfWorkFactory.Create();

        var user = await uow.UserRepository.GetAsync(userId);
        var savedRefreshToken = await uow.RefreshTokenRepository.GetByTokenAsync(refreshToken);

        if (savedRefreshToken is null ||
            savedRefreshToken.UserId != userId ||
            savedRefreshToken.ExpiresAt < DateTime.UtcNow ||
            savedRefreshToken.RevokedAt is not null ||
            user is null)
        {
            return null;
        }

        savedRefreshToken.RevokedAt = DateTime.UtcNow;
        await uow.RefreshTokenRepository.UpdateAsync(savedRefreshToken);

        var newAcessToken = GenerateAccessToken(user);

        var newRefreshToken = GenerateRefreshToken();
        newRefreshToken.UserId = userId;

        await uow.RefreshTokenRepository.AddAsync(newRefreshToken);

        uow.Commit();

        var userData = new AuthUserData
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
            DisplayName = user.DisplayName,
            Tag = user.Tag,
        };

        return new AuthResponse
        {
            ErrorType = AuthErrorType.None,
            UserData = userData
        };
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!)),
            ValidAudience = _config["JwtSettings:Audience"],
            ValidIssuer = _config["JwtSettings:Issuer"],
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
