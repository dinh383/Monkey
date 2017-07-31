#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ProcessingTimeExtensions.cs </Name>
//         <Created> 31/07/17 10:42:52 PM </Created>
//         <Key> 6c0074ee-3b9d-4c11-b552-a9e6a729a66a </Key>
//     </File>
//     <Summary>
//         ProcessingTimeExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Monkey.Extensions
{
    public static class ProcessingTimeExtensions
    {
        /// <summary>
        ///     [Response] Information about executed time 
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseProcessingTimeMonkey(this IApplicationBuilder app)
        {
            app.UseMiddleware<ProcessingTimeMiddleware>();

            return app;
        }

        public class ProcessingTimeMiddleware
        {
            private readonly RequestDelegate _next;

            public ProcessingTimeMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public Task Invoke(HttpContext context)
            {
                var watch = new Stopwatch();
                context.Response.OnStarting(state =>
                {
                    var httpContext = (HttpContext)state;
                    watch.Stop();
                    var elapsedMilliseconds = watch.ElapsedMilliseconds.ToString();
                    httpContext.Response.Headers.Add("X-Processing-Time-Milliseconds", elapsedMilliseconds);
                    return Task.CompletedTask;
                }, context);

                watch.Start();
                return _next(context);
            }
        }
    }
}