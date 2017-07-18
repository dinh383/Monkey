using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Areas.Developers.Filters;
using Monkey.Filters;
using Puppy.Web;
using Puppy.Web.Swagger;

namespace Monkey.Areas.Developers.Controllers
{
    [Area("Developers")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [ServiceFilter(typeof(DeveloperAccessFilter))]
    [Produces(ContentType.Json, ContentType.Xml)]
    [AllowAnonymous]
    [HideInDocs]
    public class DevelopersApiController : Controller
    {
    }
}