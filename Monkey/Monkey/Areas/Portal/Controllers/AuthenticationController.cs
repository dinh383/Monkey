using Microsoft.AspNetCore.Mvc;
using Monkey.Core.Models.User;

namespace Monkey.Areas.Portal.Controllers
{
    [Route(AreaName + "/auth")]
    public class AuthenticationController : MvcController
    {
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginModel());
        }

        [Route("")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Auth(LoginModel model)
        {
            return View("Index", model);
        }
    }
}