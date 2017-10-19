using Monkey.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Puppy.Core.XmlUtils;
using Puppy.Logger;
using Puppy.Web.Constants;
using System.Net;

namespace Monkey.Filters.ModelValidation
{
    public class ApiModelValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            // Log Error
            var keyValueInValid = ModelValidationHelper.GetModelStateInvalidInfo(context);
            var logMessage = JsonConvert.SerializeObject(keyValueInValid, Puppy.Core.Constants.StandardFormat.JsonSerializerSettings);
            var logId = Log.Error(logMessage);

            // Response Result
            var apiErrorViewModel = new ErrorModel(ErrorCode.BadRequest, null, keyValueInValid) { Id = logId };

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

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
                context.Result = new ContentResult
                {
                    ContentType = ContentType.Json,
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Content = JsonConvert.SerializeObject(apiErrorViewModel, Puppy.Core.Constants.StandardFormat.JsonSerializerSettings)
                };
            }
        }
    }
}