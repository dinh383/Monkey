using Microsoft.AspNetCore.Mvc;
using Monkey.Core.Exceptions;
using Monkey.Extensions;
using Puppy.Core.EnvironmentUtils;

namespace Monkey.Controllers
{
    [Route(Endpoint)]
    public class HomeController : MvcController
    {
        public const string Endpoint = AreaName;

        public const string HomeEndpoint = "";

        public const string OopsEndpoint = "oops";

        public const string OopsWithParamEndpoint = OopsEndpoint + "/{statusCode}";

        [Route(HomeEndpoint)]
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Auth", new { area = "Portal" });
        }

        [Route(OopsWithParamEndpoint)]
        [HttpGet]
        public IActionResult Oops(int statusCode)
        {
            string message = "";

            try
            {
                ErrorCode errorCode = (ErrorCode)statusCode;

                ErrorModel errorModel = new ErrorModel(errorCode, null);

                message = errorModel.Message;
            }
            catch
            {
                if (EnvironmentHelper.IsDevelopment())
                {
                    message = $"Code: {statusCode}";
                }
            }

            this.SetNotify("Oops, something went wrong!", message, NotifyStatus.Error);

            return RedirectToAction("Index", "Home");
        }
    }
}