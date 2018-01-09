using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monkey.Auth;
using Monkey.Binders;
using Monkey.Core.Configs;
using Monkey.Data.EF.Factory;
using Monkey.Extensions;
using Monkey.Mapper;
using Puppy.Core.TypeUtils;
using Puppy.DataTable;
using Puppy.DependencyInjection;
using Puppy.Hangfire;
using Puppy.Logger;
using Puppy.Redis;
using Puppy.Swagger;
using Puppy.Web.Middlewares;
using Puppy.Web.Middlewares.Cros;
using Puppy.Web.Middlewares.ServerInfo;

namespace Monkey
{
    public class Startup
    {
        public static IConfigurationRoot ConfigurationRoot;

        public static IHostingEnvironment Environment;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(Puppy.Core.Constants.Configuration.AppSettingsJsonFileName, optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            ConfigurationRoot = builder.Build();

            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                // [System Configs]
                .AddSystemConfiguration(Environment, ConfigurationRoot)

                // [HttpContext]
                .AddHttpContextAccessor()

                // [Injection]
                .AddDependencyInjection(nameof(Monkey))

                // [API Cros]
                .AddCors(ConfigurationRoot)

                // [Mapper]
                .AddAutoMapperMonkey()

                // [Background Job] Store Job in Memory. Add param
                // SystemConfigs.DatabaseConnectionString to store job in Sql Server
                .AddHangfire(ConfigurationRoot)

                // [Logger]
                .AddLogger(ConfigurationRoot)

                // [Caching]
                .AddMemoryCache()
                .AddRedisCache(ConfigurationRoot)

                // [Database]
                .AddDatabase()

                // [API Document] Swagger
                .AddApiDocument(typeof(Startup).GetAssembly(), ConfigurationRoot)
#if !DEBUG
                // [Mini Response]
                .AddMinResponse()
#endif
                // [MVC] Anti Forgery
                .AddAntiforgeryToken()

                // [Authentication] Json Web Toke + Cookie
                .AddHybridAuth(ConfigurationRoot)

                // [DataTable]
                .AddDataTable(ConfigurationRoot)

                .AddDateTimeOffsetBinder()

                // [Http Client] Flurl, see more: https://github.com/tmenier/Flurl
                .AddFlurl()

                // [Mvc - API] Json, Xml serialize, area, response caching and filters
                .AddMvcApi()

                // [Socket] SignalR
                .AddSignalR(config =>
                {
                    config.JsonSerializerSettings = Puppy.Core.Constants.StandardFormat.JsonSerializerSettings;
                });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            // [Important] The order of middleware very important for request and response handle!
            // Don't mad it !!!

            app
                // [Response] Time Executed Information
                .UseProcessingTime()

                // [HttpContext]
                .UseHttpContextAccessor()

                // [System Configs]
                .UseSystemConfiguration(loggerFactory)

                // [API Cros]
                .UseCors()

                // [Background Job] Hangfire
                .UseHangfire()

                // [Logger]
                .UseLogger(loggerFactory, appLifetime)

                // [Server Info]
                .UseServerInfo(ConfigurationRoot)

                // [API Document] Swagger
                .UseApiDocument()
#if !DEBUG

                // [Mini Response]
                .UseMinResponse()
#endif

                // [Authentication] Json Web Token + Cookie
                .UseHybridAuth()

                // [DataTable]
                .UseDataTable()

                // [Mvc - API] Static files configuration, routing [Mvc] Static files configuration, routing
                .UseMvcApi()

                // [Socket] SignalR
                .UseSignalR(routes =>
                {
                    routes.MapHub<Areas.Portal.Hubs.NotificationHub>(Areas.Portal.Hubs.NotificationHub.Url);
                });
        }
    }
}