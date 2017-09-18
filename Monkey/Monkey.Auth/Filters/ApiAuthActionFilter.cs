using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Monkey.Auth.Filters
{
    public class ApiAuthActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.IsAuthenticated())
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }

            if (!context.IsAuthorized())
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Nothing to filter in Action Executed
        }
    }
}