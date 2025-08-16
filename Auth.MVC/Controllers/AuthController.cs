using Auth.MVC.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Auth.MVC.Controllers;
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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

        var response = await _authService.LoginAsync(model);

        if (response == null)
        {
            return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 500 });
        }

        Response.Cookies.Append("access_token", response.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTimeOffset.UtcNow.AddHours(1),
            SameSite = SameSiteMode.Strict
        });

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return RedirectToAction("Index", "Home");
    }

}
