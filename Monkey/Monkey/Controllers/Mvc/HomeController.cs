using Microsoft.AspNetCore.Mvc;

namespace Monkey.Controllers.Mvc
{
    [Route(Endpoint)]
    public class HomeController : MvcController
    {
        public const string Endpoint = AreaName;

        public const string HomeEndpoint = "";

        [Route(HomeEndpoint)]
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Auth", new { area = "Portal" });
        }
    }
}