//using Auth.Core;
using Auth.WebAPI.Models;
using Auth.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(IConfiguration config)
    {
        _authService = new AuthService(config); // ← passa o IConfiguration inteiro
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (model.Username == "ivan" && model.Password == "123")
        {
            var token = _authService.GenerateToken(model.Username, "Admin", "Finance");
            return Ok(new { token });
        }

        return Unauthorized();
    }
}
