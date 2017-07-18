using Microsoft.AspNetCore.Mvc;

namespace Monkey.Controllers
{
    [Route("")]
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