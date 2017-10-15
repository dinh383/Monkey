using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Puppy.Core.XmlUtils;
using Puppy.Web.Constants;

namespace Monkey.Filters.Exception
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var errorModel = ExceptionContextHelper.GetErrorModel(context);

            // Response Result
            if (context.HttpContext.Request.Headers[HeaderKey.Accept] == ContentType.Xml || context.HttpContext.Request.Headers[HeaderKey.ContentType] == ContentType.Xml)
            {
                context.Result = new ContentResult
                {
                    ContentType = ContentType.Xml,
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Content = XmlHelper.ToXmlStringViaJson(errorModel, "Error")
                };
            }
            else
            {
                context.Result = new JsonResult(errorModel, Puppy.Core.Constants.StandardFormat.JsonSerializerSettings);
            }

            context.ExceptionHandled = true;

            // Keep base Exception
            base.OnException(context);
        }
    }
}