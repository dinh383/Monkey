using Microsoft.AspNetCore.Mvc;
using Monkey.Models.Filters;
using Puppy.Web;

namespace Monkey.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [Produces(ContentType.Json, ContentType.Xml)]
    public class ApiController : Controller
    {
    }
}