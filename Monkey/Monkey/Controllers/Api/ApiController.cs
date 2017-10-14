using Monkey.Auth.Filters;
using Monkey.Filters.Exception;
using Monkey.Filters.ModelValidation;
using Microsoft.AspNetCore.Mvc;
using Puppy.Web.Constants;

namespace Monkey.Controllers.Api
{
    [Produces(ContentType.Json, ContentType.Xml)]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [ServiceFilter(typeof(BindingLoggedInUserFilter))]
    [ServiceFilter(typeof(ApiAuthActionFilter))]
    [ServiceFilter(typeof(ApiModelValidationActionFilter))]
    public class ApiController : Controller
    {
        public const string AreaName = "api";
    }
}