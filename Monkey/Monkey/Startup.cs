using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monkey.Data;
using Monkey.Extensions;
using Monkey.Mapper;

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

                // [Caching]
                .AddCacheMonkey()

                // [Database]
                .AddDatabaseMonkey()

                // [Cros Policy]
                .AddCorsMonkey()

                // [Document API]
                .AddSwaggerMonkey()

                // [Background Job]
                .AddHangfireMonkey()

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
                // [System Configs]
                .UseSystemConfigurationMonkey(loggerFactory)

                // [Response] Time Executed Information
                .UseProcessingTimeMonkey()

                // [Response] System Information
                .UseSystemInfo()

                // [Log] Serilog
                .UseLogMonkey(loggerFactory, ConfigurationRoot)

                // [Cros] Policy
                .UseCorsMonkey()

                // [Exception]
                .UseExceptionMonkey()

                // [Security] Identity Server
                .UseIdentityServerMonkey()

                // [Document API] Swagger
                .UseSwaggerMonkey()

                // [Background Job] Hangfire
                .UseHangfireMonkey()

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
        }
    }
}