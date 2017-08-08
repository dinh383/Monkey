using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Areas.Developers.Filters;
using Puppy.Swagger.Filters;

namespace Monkey.Areas.Developers.Controllers
{
    [Area("Developers")]
    [ServiceFilter(typeof(DeveloperAccessFilter))]
    [AllowAnonymous]
    [HideInDocs]
    public class DevelopersMvcController : Controller
    {
    }
}