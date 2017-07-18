using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Web.Swagger;

namespace Monkey.Areas.Portal.Controllers
{
    [Area("Portal")]
    [HideInDocs]
    [AllowAnonymous]
    public class PortalMvcController : Controller
    {
    }
}