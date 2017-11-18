using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Monkey.Core.Exceptions;
using Puppy.Core.EnvironmentUtils;
using Puppy.Logger;
using System;
using System.Linq;
using System.Net;

namespace Monkey.Filters.Exception
{
    public static class ExceptionContextHelper
    {
        public static ErrorModel GetErrorModel(ExceptionContext context)
        {
            ErrorModel errorModel;
            MonkeyException exception = context.Exception as MonkeyException;
            string logId;

            if (exception != null)
            {
                // Log with Logger
                logId = Log.Error(context);

                context.Exception = null;

                errorModel = new ErrorModel(exception.Code, exception.Message, exception.AdditionalData);

                if (exception.AdditionalData?.Any() == true)
                {
                    errorModel.AdditionalData = exception.AdditionalData;
                }

                // Map Error Code < 600 to Response Header Code
                if ((int)exception.Code < 600)
                {
                    context.HttpContext.Response.StatusCode = (int)exception.Code;
                }
                else
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                // Log with Logger
                logId = Log.Error(context);

                errorModel = new ErrorModel(ErrorCode.Unauthorized, "Unauthorized Access");
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                // Log with Logger
                logId = Log.Fatal(context);

                var message = EnvironmentHelper.IsDevelopment()
                    ? context.Exception.Message
                    : "Oh no! You broke the system. The features do not write themselves, you know what I say, you get what you pay for....";

                errorModel = new ErrorModel(ErrorCode.Unknown, message);

                if (EnvironmentHelper.IsDevelopment())
                {
                    // Add additional data
                    errorModel.AdditionalData.Add("stackTrace", context.Exception.StackTrace);
                    errorModel.AdditionalData.Add("innerException", context.Exception.InnerException?.Message);
                    errorModel.AdditionalData.Add("note",
                        "The message is exception message and additional data such as 'stackTrace', 'internalException' and 'note' only have in [Development Environment].");
                }

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            // Update ID of error portalModel as log id
            errorModel.Id = logId;

            return errorModel;
        }
    }
}