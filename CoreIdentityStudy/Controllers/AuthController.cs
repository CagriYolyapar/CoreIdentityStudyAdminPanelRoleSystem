using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentityStudy.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AuthController : Controller
    {
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
