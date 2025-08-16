using Auth.MVC.Models;

namespace Auth.MVC.Services.Auth;

public interface IAuthService
{
    Task<TokenResponse?> LoginAsync(LoginViewModel model);
}
