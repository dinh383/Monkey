using Microsoft.AspNetCore.Mvc;

namespace Monkey.Areas.Portal.Controllers
{
    [Route("portal")]
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