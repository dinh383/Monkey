using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Swagger.Filters;

namespace Monkey.Controllers.Mvc
{
    [HideInDocs]
    [AllowAnonymous]
    public class MvcController : Controller
    {
    }
}