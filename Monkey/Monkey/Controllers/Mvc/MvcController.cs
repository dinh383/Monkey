using Microsoft.AspNetCore.Mvc;
using Monkey.Filters.Exception;
using Puppy.Swagger.Filters;

namespace Monkey.Controllers.Mvc
{
    [HideInDocs]
    [ServiceFilter(typeof(PortalMvcExceptionFilter))]
    public class MvcController : Controller
    {
        public const string AreaName = "";
    }
}