using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.MVC.Controllers;

[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        var username = User.Identity?.Name;
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var profile = User.Claims.FirstOrDefault(c => c.Type == "profile")?.Value;

        ViewBag.Username = username;
        ViewBag.Role = role;
        ViewBag.Profile = profile;

        return View();
    }
}
