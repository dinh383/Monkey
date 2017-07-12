using Microsoft.AspNetCore.Mvc;
using Monkey.Filters;
using Puppy.Web.Swagger;

namespace Monkey.Areas.Developers.Controllers
{
    [Area("Developers")]
    [ServiceFilter(typeof(DeveloperAccessFilter))]
    [HideInDocs]
    public class DevelopersMvcController : Controller
    {
    }
}