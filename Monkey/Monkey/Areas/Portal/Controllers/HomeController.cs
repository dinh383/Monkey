using Microsoft.AspNetCore.Mvc;
using Monkey.Core.Exceptions;
using Monkey.Extensions;
using Puppy.Core.EnvironmentUtils;

namespace Monkey.Areas.Portal.Controllers
{
    [Route(Endpoint)]
    public class HomeController : MvcController
    {
        public const string Endpoint = AreaName;
        public const string DashboardEndpoint = "dashboard";
        public const string OopsEndpoint = "oops";
        public const string OopsWithParamEndpoint = OopsEndpoint + "/{statusCode}";

        [Route(DashboardEndpoint)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
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