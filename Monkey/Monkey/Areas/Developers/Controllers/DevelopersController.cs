using Microsoft.AspNetCore.Mvc;
using Puppy.Swagger;
using Puppy.Swagger.Filters;
using Puppy.Web;

namespace Monkey.Areas.Developers.Controllers
{
    [Route("Developers")]
    public class DevelopersController : DevelopersMvcController
    {
        [Route("")]
        [HttpGet]
        [ServiceFilter(typeof(ApiDocAccessFilter))]
        public IActionResult Index() => Helper.GetApiDocHtml(Url, Url.AbsoluteAction("Viewer", "Developers", new { area = "Developers" }));

        [Route("Viewer")]
        [HttpGet]
        [ServiceFilter(typeof(ApiDocAccessFilter))]
        public IActionResult Viewer() => Helper.GetApiViewerHtml(Url);
    }
}