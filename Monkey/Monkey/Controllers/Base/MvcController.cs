using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Web.Swagger;

namespace Monkey.Controllers
{
    [HideInDocs]
    [AllowAnonymous]
    public class MvcController : Controller
    {
    }
}