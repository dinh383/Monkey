using Microsoft.AspNetCore.Mvc;
using Puppy.Swagger;
using Puppy.Swagger.Filters;
using Puppy.Web;

namespace Monkey.Areas.Developers.Controllers
{
    [Route(Constants.Endpoint.DevelopersArea.Developers)]
    public class DevelopersController : DevelopersMvcController
    {
        [Route("")]
        [HttpGet]
        [ServiceFilter(typeof(ApiDocAccessFilter))]
        public IActionResult Index() => Helper.GetApiDocHtml(Url,
            Url.AbsoluteAction(nameof(JsonViewer), Constants.Endpoint.DevelopersArea.Developers,
                new {area = Constants.Endpoint.DevelopersArea.Root}));

        [Route("json-viewer")]
        [HttpGet]
        [ServiceFilter(typeof(ApiDocAccessFilter))]
        public IActionResult JsonViewer() => Helper.GetApiJsonViewerHtml(Url);
    }
}