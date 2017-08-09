using Microsoft.AspNetCore.Mvc;
using Puppy.Swagger;
using Puppy.Swagger.Filters;
using Puppy.Web;

namespace Monkey.Areas.Developers.Controllers
{
    [Route("Developers")]
    public class DevelopersController : DevelopersMvcController
    {
        [Route("~/developers")]
        [HttpGet]
        [ServiceFilter(typeof(ApiDocAccessFilter))]
        public IActionResult Index() => Helper.GetApiDocHtml(Url, Url.AbsoluteAction("JsonViewer", "Developers", new { area = "Developers" }));

        [Route("~/developers/json-viewer")]
        [HttpGet]
        [ServiceFilter(typeof(ApiDocAccessFilter))]
        public IActionResult JsonViewer() => Helper.GetApiJsonViewerHtml(Url);
    }
}