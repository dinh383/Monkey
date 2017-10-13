using Monkey.Auth.Filters;
using Monkey.Auth.Filters.Attributes;
using Monkey.Filters.Exception;
using Microsoft.AspNetCore.Mvc;
using Puppy.Swagger.Filters;

namespace Monkey.Areas.Portal.Controllers
{
    [HideInDocs]
    [ServiceFilter(typeof(PortalMvcExceptionFilter))]
    [ServiceFilter(typeof(MvcAuthActionFilter))]
    [ServiceFilter(typeof(BindingLoggedInUserFilter))]
    [Area(AreaName)]
    [Auth]
    [AutoValidateAntiforgeryToken]
    public class MvcController : Controller
    {
        public const string AreaName = "portal";
    }
}