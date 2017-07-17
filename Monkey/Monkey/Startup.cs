using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Monkey.Data.EF.Factory;
using Monkey.Mapper;
using Monkey.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Monkey.Filters;
using Puppy.Web.Render;

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
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
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
            services.AddSession();
            services.AddSingleton(ConfigurationRoot);
            services.AddSingleton(Environment);

            Cros.Service(services);

            // API Doc
            Swagger.Service(services);

            // Background Job
            Hangfire.Service(services);

            Cache.Service(services);

            // Add Markup Min - Mini HTML, XML
            WebMarkupMin.Service(services);

            MapperConfiguration.Add(services);

            MapperConfiguration.Configure();

            Mvc.Service(services);

            // Keep in last
            DependencyInjection.Service(services);

            // Use Entity Framework
            services.AddDbContext<DbContext>(builder => builder.UseSqlServer(ConfigurationRoot.GetConnectionString(Environment.EnvironmentName), options => options.MigrationsAssembly(typeof(IDataModule).GetTypeInfo().Assembly.GetName().Name)));
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
        {
            // [Important] The order of middleware very important for request and response handle!
            // Don't mad it !!!

            // Currently, ASPNETCORE have a BUG hit twice when change appsetting.json from 20/03/17
            // (see more: https://github.com/aspnet/Configuration/issues/624)
            ChangeToken.OnChange(ConfigurationRoot.GetReloadToken, () => loggerFactory.CreateLogger<Startup>().LogWarning("Configuration Changed"));

            // Response Information
            ProcessingTimeMiddleware.Middleware(app);
            SystemInfoMiddleware.Middleware(app);

            // Cros
            Cros.Middleware(app);

            // Log
            Log.Middleware(app, loggerFactory);

            // Exception
            Exception.Middleware(app);

            // [Security] Identity Server
            IdentityServer.Middleware(app);

            // [Document API] Swagger
            Swagger.Middleware(app);

            // [Background Job] Hangfire
            Hangfire.Middleware(app);

            // [Mini Response]
            WebMarkupMin.Middleware(app);

            // [Final] MVC
            Mvc.Middleware(app);
        }
    }
}