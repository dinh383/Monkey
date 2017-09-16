using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Puppy.Logger;
using System;

namespace Monkey.Filters.Exception
{
    public class MvcExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException)
            {
                Log.Error(context);
                context.Result = new RedirectResult("", false); // TODO redirect to un-authorization page
            }
            else
            {
                Log.Fatal(context);
                context.Result = new RedirectResult("", false); // TODO redirect to Oops page
            }

            // Keep base Exception
            base.OnException(context);
        }
    }
}