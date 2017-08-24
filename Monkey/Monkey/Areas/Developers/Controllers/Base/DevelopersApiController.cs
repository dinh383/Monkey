using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Areas.Developers.Filters;
using Monkey.Filters;
using Puppy.Swagger.Filters;
using Puppy.Web;

namespace Monkey.Areas.Developers.Controllers.Base
{
    [Area(Constants.Endpoint.DevelopersArea.Root)]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [ServiceFilter(typeof(DeveloperAccessFilter))]
    [Produces(ContentType.Json, ContentType.Xml)]
    public class DevelopersApiController : Controller
    {
    }
}