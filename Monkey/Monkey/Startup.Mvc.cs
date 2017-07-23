using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using Monkey.Areas.Developers.Filters;
using Monkey.Filters;
using Monkey.Model.Validators;
using Puppy.Core.EnvironmentUtils;
using Puppy.Web.Render;
using System.IO;

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
                        options.SerializerSettings.ReferenceLoopHandling = Core.Constants.JsonSerializerSettings.ReferenceLoopHandling;
                        options.SerializerSettings.NullValueHandling = Core.Constants.JsonSerializerSettings.NullValueHandling;
                        options.SerializerSettings.DateTimeZoneHandling = Core.Constants.JsonSerializerSettings.DateTimeZoneHandling;
                        options.SerializerSettings.Formatting = Core.Constants.JsonSerializerSettings.Formatting;
                        options.SerializerSettings.ContractResolver = Core.Constants.JsonSerializerSettings.ContractResolver;
                    })
                    .AddViewOptions(options =>
                    {
                        // Enable Microsoft.jQuery.Unobtrusive.Validation
                        options.HtmlHelperOptions.ClientValidationEnabled = true;
                    })
                    .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<IModelValidator>());

                // Setup Areas
                services.Configure<RazorViewEngineOptions>(options =>
                {
                    options.AreaViewLocationFormats.Clear();
                    options.AreaViewLocationFormats.Add("/" + Core.SystemConfigs.MvcPath.AreasRootFolderName + "/{2}/Views/{1}/{0}.cshtml");
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
                        headers.CacheControl = new CacheControlHeaderValue
                        {
                            MaxAge = Core.SystemConfigs.MvcPath.MaxAgeResponseHeader
                        };
                    }
                });

                // Path and GZip for Statics Content
                string currentDirectory = Directory.GetCurrentDirectory();

                foreach (var staticsContent in Core.SystemConfigs.MvcPath.StaticsContents)
                {
                    string fileProviderPath = string.IsNullOrWhiteSpace(staticsContent.Area)
                        ? Path.Combine(currentDirectory, staticsContent.FolderRelativePath)
                        : Path.Combine(currentDirectory, Core.SystemConfigs.MvcPath.AreasRootFolderName,
                            staticsContent.Area,
                            staticsContent.FolderRelativePath);

                    if (!Directory.Exists(fileProviderPath))
                    {
                        // Skip if Directory is not exists
                        continue;
                    }

                    PhysicalFileProvider fileProvider = new PhysicalFileProvider(fileProviderPath);

                    PathString requestPath = new PathString(staticsContent.HttpRequestPath);

                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = fileProvider,
                        RequestPath = requestPath,
                        OnPrepareResponse = (context) =>
                        {
                            var headers = context.Context.Response.GetTypedHeaders();
                            headers.CacheControl = new CacheControlHeaderValue
                            {
                                MaxAge = staticsContent.MaxAgeResponseHeader
                            };
                        }
                    });
                }

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