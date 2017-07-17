using Microsoft.AspNetCore.Mvc;
using Monkey.Models.Filters;
using Puppy.Web;
using Puppy.Web.Swagger;

namespace Monkey.Areas.Developers.Controllers
{
    [Area("Developers")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [ServiceFilter(typeof(DeveloperAccessFilter))]
    [Produces(ContentType.Json, ContentType.Xml)]
    [HideInDocs]
    public class DevelopersApiController : Controller
    {
    }
}