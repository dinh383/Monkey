using Microsoft.AspNetCore.Mvc;
using Monkey.Filters;
using Monkey.Filters.Authorize;
using Puppy.Web.Constants;

namespace Monkey.Controllers.Api
{
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [ServiceFilter(typeof(ApiAuthorizeActionFilter))]
    [ServiceFilter(typeof(ApiModelValidationActionFilter))]
    [Produces(ContentType.Json, ContentType.Xml)]
    public class ApiController : Controller
    {
        public const string AreaName = "api";
    }
}