using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Models.Filters;
using Puppy.Web;

namespace Monkey.Areas.Portal.Controllers
{
    [Area("Portal")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [Produces(ContentType.Json, ContentType.Xml)]
    [AllowAnonymous]
    public class PortalApiController : Controller
    {
    }
}