using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Filters;

namespace Monkey.Areas.Portal.Controllers
{
    [Route("portal")]
    public class HomeController : MvcController
    {
        [Route("")]
        [HttpGet]
        [Auth]
        public IActionResult Index()
        {
            return View();
        }
    }
}