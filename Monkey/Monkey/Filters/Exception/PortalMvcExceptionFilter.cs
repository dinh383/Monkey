using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Monkey.Extensions;
using Puppy.Core.EnvironmentUtils;
using Puppy.Logger;
using Puppy.Web;
using Puppy.Web.HttpUtils;
using System;

namespace Monkey.Filters.Exception
{
    public class PortalMvcExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public PortalMvcExceptionFilter(ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public override void OnException(ExceptionContext context)
        {
            // Ajax Case
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                var errorModel = ExceptionContextHelper.GetErrorModel(context);

                context.Result = new JsonResult(errorModel, Puppy.Core.Constants.StandardFormat.JsonSerializerSettings);

                context.ExceptionHandled = true;

                // Keep base Exception
                base.OnException(context);

                return;
            }

            // MVC Page
            if (context.Exception is UnauthorizedAccessException)
            {
                Log.Error(context);

                // Redirect to un-authorization page
                context.Result = new RedirectToActionResult("Index", "Auth", new { area = "Portal" }, false);
            }
            else
            {
                Log.Fatal(context);

                // Redirect to Oops page
                context.Result = new RedirectToActionResult("Index", "Auth", new { area = "Portal" }, false);
            }

            context.ExceptionHandled = true;

            // Notify
            var tempData = _tempDataDictionaryFactory.GetTempData(context.HttpContext);

            tempData.Set(Constants.TempDataKey.Notify,
                new NotifyResultViewModel
                {
                    Title = "Something Went Wrong",
                    Message = EnvironmentHelper.IsDevelopment()
                    ? context.Exception.Message?.Replace("'", "\'")
                    : "Oh no! You broke the system. The features do not write themselves, you know what I say, you get what you pay for....",
                    Status = NotifyStatus.Error
                });

            // Keep base Exception
            base.OnException(context);
        }
    }
}