using Microsoft.AspNetCore.Mvc;

namespace Monkey.Areas.Portal.Controllers
{
    [Route(Endpoint)]
    public class HomeController : MvcController
    {
        public const string Endpoint = AreaName;
        public const string DashboardEndpoint = "";

        [Route(DashboardEndpoint)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}