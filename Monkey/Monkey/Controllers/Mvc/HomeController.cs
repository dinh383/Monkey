using Microsoft.AspNetCore.Mvc;

namespace Monkey.Controllers.Mvc
{
    [Route(Constants.Endpoint.RootArea.Home)]
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