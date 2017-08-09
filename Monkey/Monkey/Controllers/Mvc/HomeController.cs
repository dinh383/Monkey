using Microsoft.AspNetCore.Mvc;
using Puppy.Redis;

namespace Monkey.Controllers.Mvc
{
    [Route("")]
    public class HomeController : MvcController
    {
        private readonly IRedisCacheManager _redisCacheManager;

        public HomeController(IRedisCacheManager redisCacheManager)
        {
            _redisCacheManager = redisCacheManager;
        }
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Cache = _redisCacheManager.Get<string>("Test");
            return View();
        }
    }
}