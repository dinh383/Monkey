using Monkey.Auth.Filters;
using Monkey.Filters.Exception;
using Monkey.Filters.ModelValidation;
using Microsoft.AspNetCore.Mvc;
using Puppy.Web.Constants;

namespace Monkey.Controllers.Api
{
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [ServiceFilter(typeof(ApiAuthActionFilter))]
    [ServiceFilter(typeof(ApiModelValidationActionFilter))]
    [ServiceFilter(typeof(BindingLoggedInUserFilter))]
    [Produces(ContentType.Json, ContentType.Xml)]
    public class ApiController : Controller
    {
        public const string AreaName = "api";
    }
}