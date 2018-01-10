#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> MvcApiExtensions.cs </Name>
//         <Created> 31/07/17 10:42:40 PM </Created>
//         <Key> f0c4a37a-1d5d-422e-885f-38e3acce572e </Key>
//     </File>
//     <Summary>
//         MvcApiExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using Monkey.Binders;
using Monkey.Core.Configs;
using Monkey.Core.Validators;
using Monkey.Filters.Exception;
using Monkey.Filters.ModelValidation;
using Puppy.Core.EnvironmentUtils;
using Puppy.DataTable;
using Puppy.Web.Constants;
using Puppy.Web.Render;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Monkey.Extensions
{
    public static class MvcApiExtensions
    {
        /// <summary>
        ///     [Mvc - API] Json, Xml serialize, area, response caching and filters 
        /// </summary>
        /// <param name="services">         </param>
        /// <param name="configurationRoot"></param>
        public static IServiceCollection AddMvcApi(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            if (!EnvironmentHelper.IsDevelopment())
            {
                services.AddResponseCaching();
            }

            services
                // Api Filter
                .AddScoped<ApiExceptionFilter>()
                .AddScoped<ApiModelValidationActionFilter>()

                // Mvc Services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped<IViewRenderService, ViewRenderService>()

                // Mvc Filter
                .AddScoped<PortalMvcExceptionFilter>()
                .AddScoped<AjaxModelValidationActionFilter>()

                // Enable Session to use TempData
                .AddSingleton<ITempDataProvider, CookieTempDataProvider>()

                // [MVC] Anti Forgery
                .AddAntiforgeryToken()

                // [Mini Response]
#if !DEBUG
                .AddMinResponse()
#endif
                // [DataTable]
                .AddDataTable(configurationRoot)

                // [Binders]
                .AddDateTimeOffsetBinder()

                // Setup Mvc
                .AddMvc(options =>
                {
                    options.RespectBrowserAcceptHeader = false; // false by default
                    options.Filters.Add(new ProducesAttribute(ContentType.Json));
                    options.Filters.Add(new ProducesAttribute(ContentType.Xml));
                })
                .AddXmlDataContractSerializerFormatters()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Puppy.Core.Constants.StandardFormat.JsonSerializerSettings.ReferenceLoopHandling;
                    options.SerializerSettings.NullValueHandling = Puppy.Core.Constants.StandardFormat.JsonSerializerSettings.NullValueHandling;
                    options.SerializerSettings.Formatting = Puppy.Core.Constants.StandardFormat.JsonSerializerSettings.Formatting;
                    options.SerializerSettings.ContractResolver = Puppy.Core.Constants.StandardFormat.JsonSerializerSettings.ContractResolver;
                    options.SerializerSettings.DateTimeZoneHandling = Puppy.Core.Constants.StandardFormat.JsonSerializerSettings.DateTimeZoneHandling;
                    options.SerializerSettings.DateFormatString = SystemConfig.SystemDateTimeFormat;
                })
                // [Validator] Model Validator, Must after "AddMvc"
                .AddModelValidator();

            // Setup Areas
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/" + SystemConfig.MvcPath.AreasRootFolderName + "/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            return services;
        }

        /// <summary>
        ///     [Mvc - API] Static files configuration, routing [Mvc] Static files configuration, routing
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseMvcApi(this IApplicationBuilder app)
        {
            app
                // [Mini Response]
#if !DEBUG
                .UseMinResponse()
#endif
                // [DataTable]
                .UseDataTable()

                // Status Code Handle
                .UseStatusCodePages(context =>
                {
                    string requestPath = context.HttpContext.Request.Path;

                    string apiAreaRootPath = $"/{Areas.Api.Controllers.ApiController.AreaName}";

                    string portalAreaRootPath = $"/{Areas.Portal.Controllers.MvcController.AreaName}";

                    if (requestPath.StartsWith(apiAreaRootPath))
                    {
                        // Api Area

                        // Don't handle
                    }
                    else if (requestPath.StartsWith(portalAreaRootPath))
                    {
                        // Portal Area

                        // Redirect to error page
                        context.HttpContext.Response.Redirect("/");
                    }
                    else
                    {
                        // Root

                        // Redirect to error page
                        context.HttpContext.Response.Redirect("/");
                    }

                    return Task.CompletedTask;
                })

                // Root Path and GZip
                .UseStaticFiles(new StaticFileOptions
                {
                    OnPrepareResponse = (context) =>
                    {
                        var headers = context.Context.Response.GetTypedHeaders();
                        headers.CacheControl = new CacheControlHeaderValue
                        {
                            MaxAge = SystemConfig.MvcPath.MaxAgeResponseHeader
                        };
                    }
                });

            // Path and GZip for Statics Content
            string currentDirectory = Directory.GetCurrentDirectory();
            string executedAssemblyDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            if (SystemConfig.MvcPath.StaticsContents?.Any() == true)
            {
                foreach (var staticsContent in SystemConfig.MvcPath.StaticsContents)
                {
                    string fileProviderPath = string.IsNullOrWhiteSpace(staticsContent.Area)
                        ? Path.Combine(currentDirectory, staticsContent.FolderRelativePath)
                        : Path.Combine(currentDirectory, SystemConfig.MvcPath.AreasRootFolderName,
                            staticsContent.Area,
                            staticsContent.FolderRelativePath);

                    if (!Directory.Exists(fileProviderPath))
                    {
                        // Try to get folder in executed assembly
                        fileProviderPath = string.IsNullOrWhiteSpace(staticsContent.Area)
                            ? Path.Combine(executedAssemblyDirectory, staticsContent.FolderRelativePath)
                            : Path.Combine(executedAssemblyDirectory, SystemConfig.MvcPath.AreasRootFolderName,
                                staticsContent.Area,
                                staticsContent.FolderRelativePath);

                        // Skip if Directory is not exists
                        if (!Directory.Exists(fileProviderPath))
                        {
                            continue;
                        }
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
            }

            // Config Global Route
            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}");
            });

            return app;
        }

        /// <summary>
        ///     Configures the anti-forgery tokens for better security. 
        /// </summary>
        /// <param name="services"></param>
        /// <remarks> See: http://www.asp.net/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages </remarks>
        public static IServiceCollection AddAntiforgeryToken(this IServiceCollection services)
        {
            services.AddAntiforgery(
                options =>
                {
                    // Rename the Anti-Forgery cookie from "__RequestVerificationToken" to "ape".
                    // This adds a little security through obscurity and also saves sending a few
                    // characters over the wire.
                    options.Cookie.Name = "ape";

                    // Rename the form input name from "__RequestVerificationToken" to "ape" for the
                    // same reason above e.g.
                    // <input name="__RequestVerificationToken" type="hidden" value="..." />
                    options.FormFieldName = "ape";

                    // Rename the Anti-Forgery HTTP header from RequestVerificationToken to
                    // X-XSRF-TOKEN. X-XSRF-TOKEN is not a standard but a common name given to this
                    // HTTP header popularized by Angular.
                    options.HeaderName = HeaderKey.XAntiforgeryToken;

                    // If you have enabled SSL/TLS. Uncomment this line to ensure that the
                    // Anti-Forgery cookie requires SSL /TLS to be sent across the wire.
                    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                });

            return services;
        }
    }
}