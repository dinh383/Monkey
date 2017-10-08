using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Puppy.Logger;
using Puppy.Web.HttpUtils;
using System;

namespace Monkey.Filters.Exception
{
    public class MvcExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            // Ajax Case
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                var errorModel = ExceptionContextHelper.GetErrorModel(context);

                context.Result = new JsonResult(errorModel, Puppy.Core.Constants.StandardFormat.JsonSerializerSettings);

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

            // Keep base Exception
            base.OnException(context);
        }
    }
}