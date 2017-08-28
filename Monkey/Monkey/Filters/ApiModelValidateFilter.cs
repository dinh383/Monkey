using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monkey.ViewModels.Api;
using Newtonsoft.Json;
using Puppy.Core.XmlUtils;
using Puppy.Logger;
using Puppy.Web.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Monkey.Filters
{
    public class ApiModelValidateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            // Log with Logger
            var keyValueInValid = new Dictionary<string, object>();

            // Build Additional Data
            foreach (var keyValueState in context.ModelState)
            {
                var error = string.Join(", ", keyValueState.Value.Errors.Select(x => x.ErrorMessage));
                keyValueInValid.Add(keyValueState.Key, error);
            }

            var apiErrorViewModel = new ApiErrorViewModel(Core.Exceptions.ErrorCode.BadRequest, null, keyValueInValid);

            // Log Error
            var logMessage = JsonConvert.SerializeObject(keyValueInValid, Core.Constants.JsonSerializerSettings);
            var logId = Log.Error(logMessage);

            // Update ID of error model as log id
            apiErrorViewModel.Id = logId;

            // Response Result
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
                    Content = JsonConvert.SerializeObject(apiErrorViewModel, Monkey.Core.Constants.JsonSerializerSettings)
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Nothing to filter in Action Executed
        }
    }
}