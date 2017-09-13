using Microsoft.AspNetCore.Mvc;
using Puppy.Swagger.Filters;

namespace Monkey.Areas.Portal.Controllers
{
    [HideInDocs]
    [Area(AreaName)]
    public class MvcController : Controller
    {
        public const string AreaName = "portal";
    }
}