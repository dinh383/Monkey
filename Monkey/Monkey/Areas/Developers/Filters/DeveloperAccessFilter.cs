using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Monkey.Areas.Developers.Filters
{
    public class DeveloperAccessFilter : ActionFilterAttribute
    {
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
            return DeveloperHelper.IsCanAccess(context, Core.SystemConfigs.Developers.AccessKey);
        }
    }
}