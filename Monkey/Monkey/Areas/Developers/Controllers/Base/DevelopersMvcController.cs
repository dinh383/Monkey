using Microsoft.AspNetCore.Mvc;
using Monkey.Areas.Developers.Filters;

namespace Monkey.Areas.Developers.Controllers.Base
{
    [Area(Constants.Endpoint.DevelopersArea.Root)]
    [ServiceFilter(typeof(DeveloperAccessFilter))]
    public class DevelopersMvcController : Controller
    {
    }
}