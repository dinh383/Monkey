using Microsoft.AspNetCore.Mvc;

namespace Monkey.Areas.Portal.Controllers
{
    [Route(Constants.Endpoint.PortalArea.Home)]
    public class HomeController : MvcController
    {
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}