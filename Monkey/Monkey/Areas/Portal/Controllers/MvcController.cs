using Microsoft.AspNetCore.Mvc;
using Monkey.Filters.Exception;
using Puppy.Swagger.Filters;

namespace Monkey.Areas.Portal.Controllers
{
    [HideInDocs]
    [ServiceFilter(typeof(MvcExceptionFilter))]
    [Area(AreaName)]
    public class MvcController : Controller
    {
        public const string AreaName = "portal";
    }
}