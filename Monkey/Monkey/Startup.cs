using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monkey.Data;
using Monkey.Extensions;
using Monkey.Mapper;
using Puppy.DependencyInjection;
using Puppy.Hangfire;
using Puppy.Logger;
using Puppy.Redis;
using Puppy.Swagger;
using Puppy.Web.Middlewares;
using Puppy.Web.Middlewares.Cros;
using Puppy.Web.Middlewares.ServerInfo;
using System.IO;
using System.Reflection;
using Puppy.Core.TypeUtils;

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
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            ConfigurationRoot = builder.Build();

            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                // [API Cros]
                .AddCors(ConfigurationRoot)

                // [System Configs]
                .AddSystemConfigurationMonkey(Environment, ConfigurationRoot)

                // [Injection]
                .AddDependencyInjectionMonkey()

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
                .AddDatabaseMonkey()

                // [API Document] Swagger
                .AddApiDocument(typeof(Startup).GetAssembly(), ConfigurationRoot)

                // [Mini Response]
                .AddWebMarkupMinMonkey()

                // [MVC] Anti Forgery
                .AddAntiforgeryTokenMonkey()

                // [MVC]
                .AddMvcMonkey();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            // [Important] The order of middleware very important for request and response handle!
            // Don't mad it !!!

            app
                // [API Cros]
                .UseCors()

                // [Background Job] Hangfire
                .UseHangfire()

                // [Logger]
                .UseLogger(loggerFactory, appLifetime)

                // [System Configs]
                .UseSystemConfigurationMonkey(loggerFactory)

                // [Response] Time Executed Information
                .UseProcessingTime()

                // [Server Info]
                .UseServerInfo(ConfigurationRoot)

                // [Exception]
                .UseExceptionMonkey()

                // [Security] Identity Server
                .UseIdentityServerMonkey()

                // [API Document] Swagger
                .UseApiDocument()

                // [Mini Response] WebMarkup
                .UseWebMarkupMinMonkey()

                // [MVC] Keep In Last
                .UseMvcMonkey();

            // [Application Start] Initial functions
            ApplicationStart(app);
        }

        public static void ApplicationStart(IApplicationBuilder app)
        {
            // Verify Redis Setting is Fine
            IRedisCacheManager redisCacheManager = app.Resolve<IRedisCacheManager>();

            redisCacheManager.VerifySetup();

            // Migrate Database
            app.MigrateDatabase();
        }
    }
}