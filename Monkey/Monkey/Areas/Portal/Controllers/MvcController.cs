using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Filters;
using Monkey.Filters.Exception;
using Puppy.Swagger.Filters;

namespace Monkey.Areas.Portal.Controllers
{
    [HideInDocs]
    [ServiceFilter(typeof(PortalMvcExceptionFilter))]
    [ServiceFilter(typeof(MvcAuthActionFilter))]
    [Area(AreaName)]
    [Auth]
    [AutoValidateAntiforgeryToken]
    public class MvcController : Controller
    {
        public const string AreaName = "portal";
    }
}