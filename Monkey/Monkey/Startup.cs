using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monkey.Authentication;
using Monkey.Data;
using Monkey.Extensions;
using Monkey.Mapper;
using Puppy.Core.TypeUtils;
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
                .AddSystemConfiguration(Environment, ConfigurationRoot)

                // [Injection]
                .AddDependencyInjection(nameof(Monkey))

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

                // [Mini Response]
                .AddMinResponse()

                // [MVC] Anti Forgery
                .AddAntiforgeryToken()

                // [Api Filter] Model Validation, Global Exception Filer and Authorize Filter
                .AddApiFilter()

                // [Authentication]
                .AddAuthentication(ConfigurationRoot)

                // [MVC]
                .AddMvcCustom();
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
                .UseSystemConfiguration(loggerFactory)

                // [Response] Time Executed Information
                .UseProcessingTime()

                // [Server Info]
                .UseServerInfo(ConfigurationRoot)

                // [Exception]
                .UseExceptionMonkey()

                // [API Document] Swagger
                .UseApiDocument()

                // [Mini Response]
                .UseMinResponse()

                // [HttpContext]
                .UseHttpContextAccessor()

                // [Authentication]
                .UseAuthentication()

                // [MVC] Keep In Last
                .UseMvcCustom();

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

            var token = AuthenticationHelper.GenerateToken(new
            {
                userId = 123456,
                userName = "tonguyen"
            });
        }
    }
}