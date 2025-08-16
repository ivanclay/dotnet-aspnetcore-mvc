using Microsoft.AspNetCore.Mvc;

namespace Auth.MVC.Controllers;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HandleErrorCode(int statusCode)
    {
        return View("Error", statusCode);
    }
}

