using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskTracker.Domain.DTOs.Auth;
using TaskTracker.Domain.Enums;
using TaskTracker.Services.Abstraction.Interfaces.APIs;

namespace TaskTracker.WebApp.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly IAuthApi _authApi;

    public AuthController(IAuthApi authApi)
    {
        _authApi = authApi;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromForm] string login,
        [FromForm] string password,
        [FromForm] string returnUrl)
    {
        var request = new LoginRequest { Password = password };

        if (login.Contains('@'))
        {
            request.Email = login;
        }
        else
        {
            request.Tag = login;
        }

        var response = await _authApi.LoginAsync(request);

        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            return RedirectToLoginWithError(AuthErrorType.Unknown, login, returnUrl);
        }

        var content = response.Content;

        if (content.ErrorType != AuthErrorType.None)
        {
            return RedirectToLoginWithError(content.ErrorType, login, returnUrl);
        }

        if (string.IsNullOrEmpty(content.AccessToken))
        {
            return RedirectToLoginWithError(AuthErrorType.Unknown, login, returnUrl);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, login),
            new("AccessToken", content.AccessToken),
            new("RefreshToken", content.RefreshToken ?? "")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return Redirect("/");
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(
    [FromForm] SignupRequest model,
    [FromForm] string repeatPassword)
    {
        if (model.Password != repeatPassword)
        {
            return RedirectToSignupWithError(AuthErrorType.None, model, "Passwords do not match");
        }

        var response = await _authApi.SignupAsync(model);

        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            return RedirectToSignupWithError(AuthErrorType.Unknown, model);
        }

        var content = response.Content;

        if (content.ErrorType != AuthErrorType.None)
        {
            return RedirectToSignupWithError(content.ErrorType, model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, model.Email),
            new("AccessToken", content.AccessToken!),
            new("RefreshToken", content.RefreshToken ?? "")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Redirect("/");
    }

    private IActionResult RedirectToSignupWithError(AuthErrorType errorType, SignupRequest model, string customError = "")
    {
        var query = new QueryString();
        query = query.Add("errorCode", ((int)errorType).ToString());

        if (!string.IsNullOrEmpty(model.Email))
        {
            query = query.Add("email", model.Email);
        }
        if (!string.IsNullOrEmpty(model.Tag))
        {
            query = query.Add("tag", model.Tag);
        }
        if (!string.IsNullOrEmpty(model.DisplayName))
        {
            query = query.Add("displayName", model.DisplayName);
        }
        if (!string.IsNullOrEmpty(customError))
        {
            query = query.Add("customError", customError);
        }

        return Redirect("/signup" + query.ToString());
    }

    private IActionResult RedirectToLoginWithError(AuthErrorType errorType, string loginAttempt, string returnUrl)
    {
        var errorCode = (int)errorType;
        var safeLogin = Uri.EscapeDataString(loginAttempt ?? "");
        var safeReturn = Uri.EscapeDataString(returnUrl ?? "");

        return Redirect($"/login?errorCode={errorCode}&loginAttempt={safeLogin}&returnUrl={safeReturn}");
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/login");
    }
}