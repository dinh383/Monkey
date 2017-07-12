using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Monkey
{
    public partial class Startup
    {
        public class ProcessingTimeMiddleware
        {
            private readonly RequestDelegate _next;

            public ProcessingTimeMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
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
                await _next(context);
            }
        }
    }
}