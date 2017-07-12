﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monkey.Core;
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
                    options.AddPolicy(Constants.System.Cros.PolicyAllowAll, corsBuilder.Build());
                });

                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new CorsAuthorizationFilterFactory(Constants.System.Cros.PolicyAllowAll));
                });
            }

            public static void Middleware(IApplicationBuilder app)
            {
                app.UseCors(Constants.System.Cros.PolicyAllowAll);
            }

            public class ResponseMiddleware
            {
                private static readonly string AccessControlAllowOrigin =
                    ConfigurationRoot.GetValue<string>("Server:Cros:AccessControlAllowOrigin");

                private static readonly string AccessControlAllowHeaders =
                    ConfigurationRoot.GetValue<string>("Server:Cros:AccessControlAllowHeaders");

                private static readonly string AccessControlAllowMethods =
                    ConfigurationRoot.GetValue<string>("Server:Cros:AccessControlAllowMethods");

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
                            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", AccessControlAllowOrigin);

                        if (!httpContext.Response.Headers.ContainsKey("Access-Control-Allow-Headers"))
                            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", AccessControlAllowHeaders);

                        if (!httpContext.Response.Headers.ContainsKey("Access-Control-Allow-Methods"))
                            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", AccessControlAllowMethods);

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