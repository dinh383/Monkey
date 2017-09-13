using Microsoft.AspNetCore.Mvc;

namespace Monkey.Controllers.Mvc
{
    [Route("")]
    public class HomeController : MvcController
    {
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "Portal" });
        }
    }
}