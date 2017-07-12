using Microsoft.AspNetCore.Mvc;
using Monkey.Filters;
using Puppy.Web;
using Puppy.Web.Swagger;

namespace Monkey.Areas.Developers.Controllers
{
    [Area("Developers")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [ServiceFilter(typeof(DeveloperAccessFilter))]
    [HideInDocs]
    [Produces(ContentType.Json, ContentType.Xml)]
    public class DevelopersApiController : Controller
    {
    }
}