using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using Monkey.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNetCore1;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;

namespace Monkey
{
    public partial class Startup
    {
        public static class Mvc
        {
            public static void Service(IServiceCollection services)
            {
                services.AddSession();

                services.AddResponseCaching();

                services.AddWebMarkupMin(options =>
                    {
                        options.AllowMinificationInDevelopmentEnvironment = true;
                        options.DisablePoweredByHttpHeaders = true;
                        options.DisableCompression = true;
                    })
                    .AddHtmlMinification(options =>
                    {
                        options.MinificationSettings.RemoveRedundantAttributes = true;
                        options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                        options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
                        options.MinificationSettings.MinifyEmbeddedCssCode = false;
                        options.MinificationSettings.RemoveOptionalEndTags = false;
                        options.CssMinifierFactory = new NUglifyCssMinifierFactory();
                        options.JsMinifierFactory = new NUglifyJsMinifierFactory();
                    })
                    .AddXmlMinification(options =>
                    {
                        XmlMinificationSettings settings = options.MinificationSettings;
                        settings.CollapseTagsWithoutContent = true;
                    })
                    .AddHttpCompression(options =>
                    {
                        options.CompressorFactories = new List<ICompressorFactory>
                        {
                            new DeflateCompressorFactory(new DeflateCompressionSettings
                            {
                                Level = CompressionLevel.Fastest
                            }),
                            new GZipCompressorFactory(new GZipCompressionSettings
                            {
                                Level = CompressionLevel.Fastest
                            })
                        };
                    });

                services.AddMvc()
                    .AddXmlDataContractSerializerFormatters()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Core.Constants.System.JsonSerializerSettings.ReferenceLoopHandling;
                        options.SerializerSettings.NullValueHandling = Core.Constants.System.JsonSerializerSettings.NullValueHandling;
                        options.SerializerSettings.DateTimeZoneHandling = Core.Constants.System.JsonSerializerSettings.DateTimeZoneHandling;
                        options.SerializerSettings.Formatting = Core.Constants.System.JsonSerializerSettings.Formatting;
                        options.SerializerSettings.ContractResolver = Core.Constants.System.JsonSerializerSettings.ContractResolver;
                    }).AddViewOptions(options =>
                    {
                        // Enable Microsoft.jQuery.Unobtrusive.Validation
                        options.HtmlHelperOptions.ClientValidationEnabled = true;
                    });
                //.AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<IValidator>());

                services.Configure<RazorViewEngineOptions>(options =>
                {
                    options.AreaViewLocationFormats.Clear();
                    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
                });
            }

            public static void Middleware(IApplicationBuilder app, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
            {
                if (!env.IsDevelopment())
                {
                    app.UseResponseCaching();
                }

                app.UseSession();

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

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), Constants.System.WebRoot, "images", "favicons")),
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

                app.UseWebMarkupMin();
                app.UseMvc(routes =>
                {
                    routes.MapRoute("areaRoute",
                        "{area:exists}/{controller=Home}/{action=Index}");

                    routes.MapRoute(
                        "default",
                        "{controller=Home}/{action=Index}/{id?}");
                });
            }
        }
    }
}