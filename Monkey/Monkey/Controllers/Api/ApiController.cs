using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monkey.Filters;
using Puppy.Web.Constants;

namespace Monkey.Controllers.Api
{
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [Produces(ContentType.Json, ContentType.Xml)]
    [AllowAnonymous]
    public class ApiController : Controller
    {
    }
}