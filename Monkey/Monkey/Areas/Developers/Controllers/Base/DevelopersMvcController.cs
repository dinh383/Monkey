using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Areas.Developers.Filters;

namespace Monkey.Areas.Developers.Controllers.Base
{
    [AllowAnonymous]
    [Area(Constants.Endpoint.DevelopersArea.Root)]
    [ServiceFilter(typeof(DeveloperAccessFilter))]
    public class DevelopersMvcController : Controller
    {
    }
}