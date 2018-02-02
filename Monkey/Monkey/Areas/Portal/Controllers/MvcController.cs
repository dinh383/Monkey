using Microsoft.AspNetCore.Mvc;
using Monkey.Auth.Filters;
using Monkey.Auth.Filters.Attributes;
using Monkey.Filters.Exception;
using Puppy.Swagger.Filters;

namespace Monkey.Areas.Portal.Controllers
{
    [HideInDocs]
    [Auth]
    [Area(AreaName)]
    [ServiceFilter(typeof(PortalMvcExceptionFilter))]
    [ServiceFilter(typeof(LoggedInUserBinderFilter))]
    [ServiceFilter(typeof(MvcAuthActionFilter))]
    //[AutoValidateAntiforgeryToken]
    public class MvcController : Controller
    {
        public const string AreaName = "portal";
    }
}