#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> SystemConfigurationExtensions.cs </Name>
//         <Created> 31/07/17 10:43:11 PM </Created>
//         <Key> 8d82c0f2-ef77-4bdb-9ff2-8e00e66dc71f </Key>
//     </File>
//     <Summary>
//         SystemConfigurationExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Monkey.Core.Configs.Models;
using Puppy.Core.ConfigUtils;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Puppy.Core.StringUtils;
using Puppy.Web.HttpUtils;

namespace Monkey.Core.Configs
{
    public static class SystemConfigServiceExtensions
    {
        public static IServiceCollection AddSystemConfiguration(this IServiceCollection services, IHostingEnvironment hostingEnvironment, IConfigurationRoot configurationRoot)
        {
            // Add Service
            services.AddSingleton(hostingEnvironment);
            services.AddSingleton(configurationRoot);
            services.AddSingleton<IConfiguration>(configurationRoot);

            // Build System Config
            SystemConfigurationHelper.BuildSystemConfig(configurationRoot);

            return services;
        }

        public static IApplicationBuilder UseSystemConfiguration(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            // Currently, ASPNETCORE have an issue hit twice when change appsetting.json from
            // 20/03/17 (see more: https://github.com/aspnet/SystemConfigs/issues/624) And please
            // don't use IOption and IOptionSnapshot, it harder to manage lifetime of object and mad
            // of Injection I assumption configuration is singleton and use Statics class to do it.
            // Keep Simple everything Possible.

            IConfigurationRoot configurationRoot = app.ApplicationServices.GetService<IConfigurationRoot>();

            ChangeToken.OnChange(configurationRoot.GetReloadToken, () =>
            {
                // Build System Config
                SystemConfigurationHelper.BuildSystemConfig(configurationRoot);
            });

            return app;
        }
    }

    public static class SystemConfigurationHelper
    {
        public static void BuildSystemConfig(IConfiguration configuration)
        {
            // Connection String
            // Connection String
            SystemConfig.DatabaseConnectionString = configuration.GetValueByMachineAndEnv<string>("ConnectionStrings");
            SystemConfig.LogDatabaseConnectionString = configuration.GetValueByMachineAndEnv<string>("LogConnectionStrings");
            SystemConfig.IsUseLogDatabase = configuration.GetValueByMachineAndEnv<bool>(nameof(SystemConfig.IsUseLogDatabase));

            SystemConfig.MvcPath = configuration.GetSection<MvcPathConfigModel>(nameof(SystemConfig.MvcPath)) ?? new MvcPathConfigModel();

            // Time Zone
            SystemConfig.SystemTimeZone = configuration.GetValue(nameof(SystemConfig.SystemTimeZone), SystemConfig.SystemTimeZone);
            SystemUtils.SystemTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(SystemConfig.SystemTimeZone);

            // Others
            SystemConfig.PagedCollectionParameters = configuration.GetSection<PagedCollectionParametersConfigModel>(nameof(SystemConfig.PagedCollectionParameters)) ?? new PagedCollectionParametersConfigModel();
            SystemConfig.SendGrid = configuration.GetSection<SendGridConfigModel>(nameof(SystemConfig.SendGrid)) ?? new SendGridConfigModel();
        }
    }

    public class SystemDomainMiddleware
    {
        private readonly RequestDelegate _next;

        public SystemDomainMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            SystemConfig.SystemDomainUrl = context.Request.GetDomain();

            // Make sure System Domain Url not end by /
            SystemConfig.SystemDomainUrl = SystemConfig.SystemDomainUrl?.CleanUrlPath();

            return _next(context);
        }
    }
}