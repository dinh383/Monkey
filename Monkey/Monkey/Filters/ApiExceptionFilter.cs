using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monkey.Core.Exceptions;
using Monkey.ViewModels.Api;
using Puppy.Core.XmlUtils;
using Puppy.Logger;
using Puppy.Web;
using System;
using System.Net;
using Puppy.Logger.Log;

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
                // Unhandled errors
#if !DEBUG
                var msg = "An unhandled error occurred.";
#else
                var msg = context.Exception.GetBaseException().Message + Environment.NewLine + context.Exception.StackTrace;
#endif

                apiErrorViewModel = new ApiErrorViewModel(ErrorCode.Unknown, msg);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            // Log
            string logId = Log.Error(context);

            // Update ID of error model as log id
            apiErrorViewModel.Id = logId;

            // Response
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

            base.OnException(context);
        }
    }
}