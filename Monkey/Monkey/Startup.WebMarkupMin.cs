using System.Collections.Generic;
using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNetCore1;
using WebMarkupMin.NUglify;

namespace Monkey
{
    public partial class Startup
    {
        public static class WebMarkupMin
        {
            public static void Service(IServiceCollection services)
            {
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
                        options.MinificationSettings.MinifyEmbeddedCssCode = true;
                        options.MinificationSettings.RemoveOptionalEndTags = true;
                        options.CssMinifierFactory = new NUglifyCssMinifierFactory();
                        options.JsMinifierFactory = new NUglifyJsMinifierFactory();
                    })
                    .AddXmlMinification(options =>
                    {
                        options.MinificationSettings.CollapseTagsWithoutContent = true;
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
            }

            public static void Middleware(IApplicationBuilder app)
            {
                app.UseWebMarkupMin();
            }
        }
    }
}