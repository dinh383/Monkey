using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using Monkey.Areas.Developers.Filters;
using Monkey.Model.Validators;
using Puppy.Core;
using Puppy.Web.Render;
using System;
using System.IO;
using Monkey.Filters;

namespace Monkey
{
    public partial class Startup
    {
        public static class Mvc
        {
            public static void Service(IServiceCollection services)
            {
                // Utility Services
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddSingleton<IViewRenderService, ViewRenderService>();
                services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
                services.AddScoped<DeveloperAccessFilter>();
                services.AddScoped<ApiExceptionFilter>();

                if (EnvironmentHelper.IsProduction())
                {
                    services.AddResponseCaching();
                }

                // Setup Mvc
                services.AddMvc()
                    .AddXmlDataContractSerializerFormatters()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Core.Constants.Setting.JsonSerializerSettings.ReferenceLoopHandling;
                        options.SerializerSettings.NullValueHandling = Core.Constants.Setting.JsonSerializerSettings.NullValueHandling;
                        options.SerializerSettings.DateTimeZoneHandling = Core.Constants.Setting.JsonSerializerSettings.DateTimeZoneHandling;
                        options.SerializerSettings.Formatting = Core.Constants.Setting.JsonSerializerSettings.Formatting;
                        options.SerializerSettings.ContractResolver = Core.Constants.Setting.JsonSerializerSettings.ContractResolver;
                    })
                    .AddViewOptions(options =>
                    {
                        // Enable Microsoft.jQuery.Unobtrusive.Validation
                        options.HtmlHelperOptions.ClientValidationEnabled = true;
                    })
                    .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<IValidator>());

                // Setup Areas
                services.Configure<RazorViewEngineOptions>(options =>
                {
                    options.AreaViewLocationFormats.Clear();
                    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
                });
            }

            public static void Middleware(IApplicationBuilder app)
            {
                if (EnvironmentHelper.IsProduction())
                {
                    app.UseResponseCaching();
                }

                // Root Path and GZip
                app.UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = (context) =>
                    {
                        var headers = context.Context.Response.GetTypedHeaders();
                        headers.CacheControl = new CacheControlHeaderValue()
                        {
                            MaxAge = TimeSpan.FromDays(365)
                        };
                    }
                });

                // Favicons Path and GZip
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), Core.Constants.Setting.WebRoot, "favicons")),
                    RequestPath = new PathString("/favicons"),
                    OnPrepareResponse = (context) =>
                    {
                        var headers = context.Context.Response.GetTypedHeaders();
                        headers.CacheControl = new CacheControlHeaderValue()
                        {
                            MaxAge = TimeSpan.FromDays(365)
                        };
                    }
                });

                // Developer Template Path and GZip
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Areas", "Developers", "SwaggerTemplate")),
                    RequestPath = new PathString("/developers/template"),
                    OnPrepareResponse = (context) =>
                    {
                        var headers = context.Context.Response.GetTypedHeaders();
                        headers.CacheControl = new CacheControlHeaderValue()
                        {
                            MaxAge = TimeSpan.FromDays(365)
                        };
                    }
                });

                // Portal Asset Path and GZip
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Areas", "Portal", "Assets")),
                    RequestPath = new PathString("/portal/assets"),
                    OnPrepareResponse = (context) =>
                    {
                        var headers = context.Context.Response.GetTypedHeaders();
                        headers.CacheControl = new CacheControlHeaderValue()
                        {
                            MaxAge = TimeSpan.FromDays(365)
                        };
                    }
                });

                // Config Global Route
                app.UseMvc(routes =>
                {
                    routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}");

                    routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                });
            }
        }
    }
}