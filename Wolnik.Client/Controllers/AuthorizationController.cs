using Microsoft.AspNetCore.Mvc;

namespace Wolnik.Client.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}