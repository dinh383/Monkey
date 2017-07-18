using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monkey.Mapper;

namespace Monkey
{
    public partial class Startup
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
            // [SystemConfigs]
            SystemConfigs.Service(services);

            // [Cros] Policy
            Cros.Service(services);

            // [Document API] Swagger
            Swagger.Service(services);

            // [Background Job] Hangfire
            Hangfire.Service(services);

            // [Caching] Redis Cache
            Cache.Service(services);

            // [Mini Response] WebMarkup
            WebMarkupMin.Service(services);

            // [Mapper] Auto Mapper
            MapperConfiguration.Service(services);

            // [MVC]
            Mvc.Service(services);

            // [Injection] Keep In Last
            DependencyInjection.Service(services);

            // [Database] Use Entity Framework
            Database.Service(services);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            // [Important] The order of middleware very important for request and response handle!
            // Don't mad it !!!

            // [SystemConfigs]
            SystemConfigs.Middleware(app, loggerFactory);

            // [Response] Information
            ProcessingTimeMiddleware.Middleware(app);
            SystemInfoMiddleware.Middleware(app);

            // [Cros] Policy
            Cros.Middleware(app);

            // [Log] Serilog
            Log.Middleware(app, loggerFactory);

            // [Exception]
            Exception.Middleware(app);

            // [Security] Identity Server
            IdentityServer.Middleware(app);

            // [Document API] Swagger
            Swagger.Middleware(app);

            // [Background Job] Hangfire
            Hangfire.Middleware(app);

            // [Mini Response] WebMarkup
            WebMarkupMin.Middleware(app);

            // [MVC] Keep In Last
            Mvc.Middleware(app);
        }
    }
}