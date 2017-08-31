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
using Monkey.Core.ConfigModels;
using Puppy.Core.ConfigUtils;
using Puppy.DependencyInjection;

namespace Monkey.Extensions
{
    public static class SystemConfigurationExtensions
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

            IConfigurationRoot configurationRoot = app.Resolve<IConfigurationRoot>();

            ChangeToken.OnChange(configurationRoot.GetReloadToken, () =>
            {
                // Build System Config
                SystemConfigurationHelper.BuildSystemConfig(configurationRoot);

                loggerFactory.CreateLogger<Startup>().LogWarning("System Configuration Changed!");
            });

            return app;
        }
    }

    public static class SystemConfigurationHelper
    {
        public static void BuildSystemConfig(IConfiguration configuration)
        {
            Core.SystemConfigs.DatabaseConnectionString = configuration.GetValueByMachineAndEnv<string>("ConnectionStrings");
            Core.SystemConfigs.MvcPath = configuration.GetSection<MvcPathConfigModel>(nameof(Core.SystemConfigs.MvcPath)) ?? new MvcPathConfigModel();
            Core.SystemConfigs.IdentityServer = configuration.GetSection<IdentityServerConfigModel>(nameof(Core.SystemConfigs.IdentityServer)) ?? new IdentityServerConfigModel();
            Core.SystemConfigs.PagedCollectionParameters = configuration.GetSection<PagedCollectionParametersConfigModel>(nameof(Core.SystemConfigs.PagedCollectionParameters)) ?? new PagedCollectionParametersConfigModel();
        }
    }
}