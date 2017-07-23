using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Monkey
{
    public partial class Startup
    {
        public static class Cros
        {
            public static void Service(IServiceCollection services)
            {
                var corsBuilder = new CorsPolicyBuilder();
                corsBuilder.AllowAnyOrigin();
                corsBuilder.AllowAnyHeader();
                corsBuilder.AllowAnyMethod();
                corsBuilder.AllowCredentials();

                services.AddCors(options =>
                {
                    options.AddPolicy(Core.SystemConfigs.Server.Cros.PolicyAllowAllName, corsBuilder.Build());
                });

                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new CorsAuthorizationFilterFactory(Core.SystemConfigs.Server.Cros.PolicyAllowAllName));
                });
            }

            public static void Middleware(IApplicationBuilder app)
            {
                app.UseCors(Core.SystemConfigs.Server.Cros.PolicyAllowAllName);
                app.UseMiddleware<ResponseMiddleware>();
            }

            /// <summary>
            ///     This middleware for hot fix API bug of current AspNetCore 
            /// </summary>
            public class ResponseMiddleware
            {
                private readonly RequestDelegate _next;

                public ResponseMiddleware(RequestDelegate next)
                {
                    _next = next;
                }

                public Task Invoke(HttpContext context)
                {
                    context.Response.OnStarting(state =>
                    {
                        var httpContext = (HttpContext)state;
                        if (!httpContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", Core.SystemConfigs.Server.Cros.AccessControlAllowOrigin);

                        if (!httpContext.Response.Headers.ContainsKey("Access-Control-Allow-Headers"))
                            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", Core.SystemConfigs.Server.Cros.AccessControlAllowHeaders);

                        if (!httpContext.Response.Headers.ContainsKey("Access-Control-Allow-Methods"))
                            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", Core.SystemConfigs.Server.Cros.AccessControlAllowMethods);

                        if (httpContext.Request.Method.Equals("OPTIONS", StringComparison.Ordinal))
                            httpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;

                        return Task.CompletedTask;
                    }, context);

                    return _next(context);
                }
            }
        }
    }
}