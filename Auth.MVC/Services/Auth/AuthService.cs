using Auth.MVC.Models;
using Newtonsoft.Json;

namespace Auth.MVC.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TokenResponse?> LoginAsync(LoginViewModel model)
    {
        var client = _httpClientFactory.CreateClient("AuthApi");

        var response = await client.PostAsJsonAsync("api/Auth/login", model);

        if (!response.IsSuccessStatusCode)
            return null;

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

        return tokenResponse;
    }
}

