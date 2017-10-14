using Monkey.Auth.Filters;
using Monkey.Auth.Filters.Attributes;
using Monkey.Filters.Exception;
using Microsoft.AspNetCore.Mvc;
using Puppy.Swagger.Filters;

namespace Monkey.Areas.Portal.Controllers
{
    [HideInDocs]
    [Area(AreaName)]
    [Auth]
    [ServiceFilter(typeof(PortalMvcExceptionFilter))]
    [ServiceFilter(typeof(BindingLoggedInUserFilter))]
    [ServiceFilter(typeof(MvcAuthActionFilter))]
    public class MvcController : Controller
    {
        public const string AreaName = "portal";
    }
}