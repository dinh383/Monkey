using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Areas.Developers.Filters;
using Puppy.Swagger.Filters;

namespace Monkey.Areas.Developers.Controllers
{
    [AllowAnonymous]
    [HideInDocs]
    [Area(Constants.Endpoint.DevelopersArea.Root)]
    [ServiceFilter(typeof(DeveloperAccessFilter))]
    public class DevelopersMvcController : Controller
    {
    }
}