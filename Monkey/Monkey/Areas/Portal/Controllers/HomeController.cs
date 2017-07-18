using Microsoft.AspNetCore.Mvc;

namespace Monkey.Areas.Portal.Controllers
{
    [Route("portal")]
    public class HomeController : PortalMvcController
    {
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}