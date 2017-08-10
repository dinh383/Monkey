using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monkey.Core;
using Monkey.Data;
using Monkey.Extensions;
using Monkey.Mapper;
using Puppy.DependencyInjection;
using Puppy.Hangfire;
using Puppy.Logger;
using Puppy.Redis;
using Puppy.Swagger;
using System;
using System.IO;

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
                // [System Configs]
                .AddSystemConfigurationMonkey(Environment, ConfigurationRoot)

                // [Injection]
                .AddDependencyInjectionMonkey()

                // [Mapper]
                .AddAutoMapperMonkey()

                // [Logger]
                .AddLogger(ConfigurationRoot)

                // [Caching]
                .AddMemoryCache()
                .AddRedisCache(ConfigurationRoot)

                // [Database]
                .AddDatabaseMonkey()

                // [Cros Policy]
                .AddCorsMonkey()

                // [API Document] Swagger
                .AddApiDocument(Path.Combine(Directory.GetCurrentDirectory(), "Documentation.xml"), ConfigurationRoot)

                // [Background Job]
                .AddHangfire(SystemConfigs.DatabaseConnectionString, ConfigurationRoot)

                // [Mini Response]
                .AddWebMarkupMinMonkey()

                // [MVC] Anti Forgery
                .AddAntiforgeryTokenMonkey()

                // [MVC]
                .AddMvcMonkey();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            // [Important] The order of middleware very important for request and response handle!
            // Don't mad it !!!

            app
                // [Logger]
                .UseLogger(loggerFactory)

                // [System Configs]
                .UseSystemConfigurationMonkey(loggerFactory)

                // [Response] Time Executed Information
                .UseProcessingTimeMonkey()

                // [Response] System Information
                .UseSystemInfo()

                // [Cros] Policy
                .UseCorsMonkey()

                // [Exception]
                .UseExceptionMonkey()

                // [Security] Identity Server
                .UseIdentityServerMonkey()

                // [API Document] Swagger
                .UseApiDocument()

                // [Background Job] Hangfire
                .UseHangfire()

                // [Mini Response] WebMarkup
                .UseWebMarkupMinMonkey()

                // [MVC] Keep In Last
                .UseMvcMonkey();

            // [Application Start] Initial functions
            ApplicationStart(app);
        }

        public static void ApplicationStart(IApplicationBuilder app)
        {
            // Migrate Database
            app.MigrateDatabase();

            IRedisCacheManager redisCacheManager = app.Resolve<IRedisCacheManager>();
            redisCacheManager.Set("Test", "Test Cache Data", TimeSpan.FromDays(1));
        }
    }
}