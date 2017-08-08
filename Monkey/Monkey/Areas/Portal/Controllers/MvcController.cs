using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Swagger.Filters;

namespace Monkey.Areas.Portal.Controllers
{
    [Area("Portal")]
    [HideInDocs]
    [AllowAnonymous]
    public class MvcController : Controller
    {
    }
}