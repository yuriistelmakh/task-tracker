using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.APIs;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services;

public class AuthService : IAuthService
{
    private readonly IAuthApi _authApi;

    public AuthService(IAuthApi authApi)
    {
        _authApi = authApi;
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var response = await _authApi.LoginAsync(request);

        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            return AuthResult.Failure(AuthErrorType.Unknown);
        }

        var content = response.Content;

        if (content.ErrorType != AuthErrorType.None)
        {
            return AuthResult.Failure(content.ErrorType);
        }

        if (string.IsNullOrWhiteSpace(content.AccessToken))
        {
            return AuthResult.Failure(AuthErrorType.Unknown);
        }
         

        return AuthResult.Success(content.AccessToken!);
    }


    public async Task<AuthResult> SignupAsync(SignupRequest request)
    {
        var response = await _authApi.SignupAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return AuthResult.Failure(response.Content?.ErrorType ?? AuthErrorType.Unknown);
        }

        if (response.Content!.ErrorType != AuthErrorType.None)
        {
            return AuthResult.Failure(response.Content.ErrorType);
        }

        if (response.Content is null)
        {
            return AuthResult.Failure(AuthErrorType.Unknown);
        }

        var content = response.Content;


        if (string.IsNullOrWhiteSpace(content.AccessToken))
        {
            return AuthResult.Failure(AuthErrorType.Unknown);
        }

        return AuthResult.Success(content.AccessToken!);
    }
}
