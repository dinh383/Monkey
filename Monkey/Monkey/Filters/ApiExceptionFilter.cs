using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monkey.Core.Exceptions;
using Monkey.ViewModels.Api;
using Newtonsoft.Json;
using Puppy.Core.XmlUtils;
using Puppy.Logger;
using Puppy.Web;
using System;
using System.Net;

namespace Monkey.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ApiErrorViewModel apiErrorViewModel;
            var exception = context.Exception as MonkeyException;

            if (exception != null)
            {
                var ex = exception;
                context.Exception = null;
                apiErrorViewModel = new ApiErrorViewModel(ex.Code, ex.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                apiErrorViewModel = new ApiErrorViewModel(ErrorCode.Unauthorized, "Unauthorized Access");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
#if DEBUG
                object debugMessage = new
                {
                    context.Exception.Message,
                    context.Exception.StackTrace,
                    InternalMessage = context.Exception.InnerException?.Message
                };

                var msg = JsonConvert.SerializeObject(debugMessage, Formatting.Indented, Core.Constants.JsonSerializerSettings);
#else
                // Friendly message for Publish version.
                var msg = "Oh no! You broke the system. The features do not write themselves, you know what I say, you get what you pay for....";
#endif

                apiErrorViewModel = new ApiErrorViewModel(ErrorCode.Unknown, msg);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            // Log with Logger
            string logId = Log.Error(context);

            // Update ID of error model as log id
            apiErrorViewModel.Id = logId;

            // Response Result
            if (context.HttpContext.Request.Headers[HttpRequestHeader.Accept.ToString()] == ContentType.Xml)
            {
                context.Result = new ContentResult
                {
                    ContentType = ContentType.Xml,
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Content = XmlHelper.ToXmlString(apiErrorViewModel)
                };
            }
            else
            {
                context.Result = new JsonResult(apiErrorViewModel, Core.Constants.JsonSerializerSettings);
            }

            // Keep base Exception
            base.OnException(context);
        }
    }
}