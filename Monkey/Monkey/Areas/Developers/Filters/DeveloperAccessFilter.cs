using Microsoft.AspNetCore.Mvc.Filters;

namespace Monkey.Areas.Developers.Filters
{
    public class DeveloperAccessFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
    }
}