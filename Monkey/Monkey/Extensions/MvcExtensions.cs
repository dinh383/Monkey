﻿#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> MvcExtensions.cs </Name>
//         <Created> 31/07/17 10:42:40 PM </Created>
//         <Key> f0c4a37a-1d5d-422e-885f-38e3acce572e </Key>
//     </File>
//     <Summary>
//         MvcExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

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

namespace Monkey.Extensions
{
    public static class MvcExtensions
    {
        /// <summary>
        ///     [Mvc] Json, Xml serialize, area and response caching 
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddMvcMonkey(this IServiceCollection services)
        {
            // Mvc Services
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IViewRenderService, ViewRenderService>();

            // Filters
            services.AddScoped<DeveloperAccessFilter>();
            services.AddScoped<ApiExceptionFilter>();

            if (EnvironmentHelper.IsProduction())
            {
                services.AddResponseCaching();
            }

            // Setup Mvc
            services
                .AddMvc()
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

            return services;
        }

        /// <summary>
        ///     Configures the anti-forgery tokens for better security. 
        /// </summary>
        /// <param name="services"></param>
        /// <remarks> See: http://www.asp.net/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages </remarks>
        public static IServiceCollection AddAntiforgeryTokenMonkey(this IServiceCollection services)
        {
            services.AddAntiforgery(
                options =>
                {
                    // Rename the Anti-Forgery cookie from "__RequestVerificationToken" to "ape".
                    // This adds a little security through obscurity and also saves sending a few
                    // characters over the wire.
                    options.CookieName = "ape";

                    // Rename the form input name from "__RequestVerificationToken" to "ape" for the
                    // same reason above e.g.
                    // <input name="__RequestVerificationToken" type="hidden" value="..." />
                    options.FormFieldName = "ape";

                    // Rename the Anti-Forgery HTTP header from RequestVerificationToken to
                    // X-XSRF-TOKEN. X-XSRF-TOKEN is not a standard but a common name given to this
                    // HTTP header popularized by Angular.
                    options.HeaderName = "X-XSRF-TOKEN";

                    // If you have enabled SSL/TLS. Uncomment this line to ensure that the
                    // Anti-Forgery cookie requires SSL /TLS to be sent across the wire.
                    options.RequireSsl = true;
                });

            return services;
        }

        /// <summary>
        ///     [Mvc] Static files configuration, routing [Mvc] Static files configuration, routing 
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseMvcMonkey(this IApplicationBuilder app)
        {
            if (EnvironmentHelper.IsProduction())
            {
                app.UseResponseCaching();
            }
            else
            {
                app.UseBrowserLink();
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

            return app;
        }
    }
}