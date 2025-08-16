using Auth.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Auth.MVC.Controllers;
public class AuthController : Controller
{
    private readonly HttpClient _httpClient;

    public AuthController()
    
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://suaapi.com.br/");
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var payload = new
        {
            username = model.UserName,
            password = model.Password
        };

        var response = await _httpClient.PostAsJsonAsync("https://localhost:7138/api/Auth/login", payload);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            // Armazenar token em cookie
            Response.Cookies.Append("access_token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                SameSite = SameSiteMode.Strict
            });


            return RedirectToAction("Index", "Dashboard");
        }

        //model.Erro = "Credenciais inválidas";
        return View(model);
    }
}
