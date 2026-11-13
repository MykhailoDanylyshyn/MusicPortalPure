using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MusicPortal.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(IConfiguration configuration) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var spaUrl = configuration["AdminSpaUrl"] ?? "https://localhost:7202";
        return Redirect(spaUrl);
    }
}
