using Monkey.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Puppy.Logger;
using Puppy.Web.Constants;
using Puppy.Web.HttpUtils;
using System.Net;

namespace Monkey.Filters.ModelValidation
{
    public class AjaxModelValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            // This action filter only work for Ajax request
            if (!context.HttpContext.Request.IsAjaxRequest())
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

            context.Result = new ContentResult
            {
                ContentType = ContentType.Json,
                StatusCode = context.HttpContext.Response.StatusCode,
                Content = JsonConvert.SerializeObject(apiErrorViewModel, Puppy.Core.Constants.StandardFormat.JsonSerializerSettings)
            };
        }
    }
}