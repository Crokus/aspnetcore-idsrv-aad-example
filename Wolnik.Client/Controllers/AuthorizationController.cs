using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Wolnik.Client.Controllers {
    public class AuthorizationController : Controller {
        public IActionResult AccessDenied() {
            return View();
        }

        public async Task Logout() {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}
