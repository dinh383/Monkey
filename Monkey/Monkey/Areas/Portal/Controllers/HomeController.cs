using Microsoft.AspNetCore.Mvc;
using Monkey.Filters.Authorize;

namespace Monkey.Areas.Portal.Controllers
{
    [Route("portal")]
    public class HomeController : MvcController
    {
        [Route("")]
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}