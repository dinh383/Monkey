using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monkey.Core.Exceptions;
using Monkey.ViewModels.Api;
using Puppy.Core.EnvironmentUtils;
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
            MonkeyException exception = context.Exception as MonkeyException;
            string logId;

            if (exception != null)
            {
                // Log with Logger
                logId = Log.Error(context);

                var ex = exception;
                context.Exception = null;
                apiErrorViewModel = new ApiErrorViewModel(ex.Code, ex.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                // Log with Logger
                logId = Log.Error(context);

                apiErrorViewModel = new ApiErrorViewModel(ErrorCode.Unauthorized, "Unauthorized Access");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                // Log with Logger
                logId = Log.Fatal(context);

                var message = EnvironmentHelper.IsDevelopment()
                    ? context.Exception.Message
                    : "Oh no! You broke the system. The features do not write themselves, you know what I say, you get what you pay for....";

                apiErrorViewModel = new ApiErrorViewModel(ErrorCode.Unknown, message);

                if (EnvironmentHelper.IsDevelopment())
                {
                    // Add additional data
                    apiErrorViewModel.AdditionalData.Add("stackTrace", context.Exception.StackTrace);
                    apiErrorViewModel.AdditionalData.Add("innerException", context.Exception.InnerException?.Message);
                    apiErrorViewModel.AdditionalData.Add("note", "The message is exception message and additional data such as 'stackTrace', 'internalException' and 'note' only have in [Development Environment].");
                }

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

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