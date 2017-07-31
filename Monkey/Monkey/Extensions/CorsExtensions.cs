#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> CorsExtensions.cs </Name>
//         <Created> 31/07/17 10:34:32 PM </Created>
//         <Key> 6a516945-9198-4e0e-b511-419d42705499 </Key>
//     </File>
//     <Summary>
//         CorsExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.DependencyInjection;
using Monkey.Core;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Monkey.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsMonkey(this IServiceCollection services)
        {
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy(SystemConfigs.Server.Cros.PolicyAllowAllName, corsBuilder.Build());
            });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(SystemConfigs.Server.Cros.PolicyAllowAllName));
            });

            return services;
        }

        public static IApplicationBuilder UseCorsMonkey(this IApplicationBuilder app)
        {
            app.UseCors(SystemConfigs.Server.Cros.PolicyAllowAllName);
            app.UseMiddleware<CorsMiddlewareMonkey>();

            return app;
        }

        /// <summary>
        ///     This middleware for hot fix current issue of AspNetCore Cors 
        /// </summary>
        public class CorsMiddlewareMonkey
        {
            private readonly RequestDelegate _next;

            public CorsMiddlewareMonkey(RequestDelegate next)
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