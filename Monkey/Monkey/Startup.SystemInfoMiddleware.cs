using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Monkey
{
    public partial class Startup
    {
        public class SystemInfoMiddleware
        {
            private static readonly string AuthorWebsite = ConfigurationRoot.GetValue<string>("Server:AuthorWebsite");
            private static readonly string AuthorName = ConfigurationRoot.GetValue<string>("Server:AuthorName");
            private static readonly string PoweredBy = ConfigurationRoot.GetValue<string>("Server:PoweredBy");
            private static readonly string ServerName = ConfigurationRoot.GetValue<string>("Server:Name");

            private readonly RequestDelegate _next;

            public SystemInfoMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                context.Response.OnStarting(state =>
                {
                    var httpContext = (HttpContext)state;

                    if (httpContext.Response.Headers.ContainsKey("Server"))
                        httpContext.Response.Headers.Remove("Server");
                    httpContext.Response.Headers.Add("Server", ServerName);

                    if (httpContext.Response.Headers.ContainsKey("X-Powered-By"))
                        httpContext.Response.Headers.Remove("X-Powered-By");
                    httpContext.Response.Headers.Add("X-Powered-By", PoweredBy);

                    if (httpContext.Response.Headers.ContainsKey("X-Author-Name"))
                        httpContext.Response.Headers.Remove("X-Author-Name");
                    httpContext.Response.Headers.Add("X-Author-Name", AuthorName);

                    if (httpContext.Response.Headers.ContainsKey("X-Author-Website"))
                        httpContext.Response.Headers.Remove("X-Author-Website");
                    httpContext.Response.Headers.Add("X-Author-Website", AuthorWebsite);

                    return Task.CompletedTask;
                }, context);

                await _next(context);
            }
        }
    }
}