using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Swagger.Filters;

namespace Monkey.Areas.Portal.Controllers
{
    [AllowAnonymous]
    [HideInDocs]
    [Area(Constants.Endpoint.PortalArea.Root)]
    public class MvcController : Controller
    {
    }
}