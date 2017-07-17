using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Monkey.Models.Filters
{
    public class DeveloperAccessFilter : ActionFilterAttribute
    {
        private readonly IConfigurationRoot _configurationRoot;

        public DeveloperAccessFilter(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsValidDeveloperRequest(context.HttpContext))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.HttpContext.Response.Headers.Clear();
                context.Result = new EmptyResult();
                return;
            }
            base.OnActionExecuting(context);
        }

        private bool IsValidDeveloperRequest(HttpContext context)
        {
            var developerKey = _configurationRoot.GetValue<string>("Developers:AccessKey");
            return DeveloperHelper.IsCanAccess(context, developerKey);
        }
    }
}