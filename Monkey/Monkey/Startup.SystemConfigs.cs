using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Puppy.Core;

namespace Monkey
{
    public partial class Startup
    {
        public static class SystemConfigs
        {
            /// <summary>
            ///     I will add config into 2 places: <br />
            ///     - First place is in SystemConfig object: this will content all config. <br />
            ///     - Second place is in separate object: this for lightweight inject and read data.
            /// </summary>
            /// <param name="services"></param>
            public static void Service(IServiceCollection services)
            {
                // Add Service
                services.AddSingleton(Environment);
                services.AddSingleton(ConfigurationRoot);
                services.AddSingleton<IConfiguration>(ConfigurationRoot);

                // Build System Config
                BuildSystemConfig();
            }

            public static void Middleware(IApplicationBuilder app, ILoggerFactory loggerFactory)
            {
                // Currently, ASPNETCORE have a BUG hit twice when change appsetting.json from
                // 20/03/17 (see more: https://github.com/aspnet/SystemConfigs/issues/624) And please
                // don't use IOption and IOptionSnapshot, it harder to manage lifetime of object and
                // mad of Injection I assumption configuration is singleton and use Statics class to
                // do it. Keep Simple everything Possible.

                ChangeToken.OnChange(ConfigurationRoot.GetReloadToken, () =>
                {
                    // Build System Config
                    BuildSystemConfig();

                    loggerFactory.CreateLogger<Startup>().LogWarning($"{nameof(Core.SystemConfigs)} Changed!");
                });
            }

            private static void BuildSystemConfig()
            {
                // Database Connection String
                string databaseConnectionString =
                    ConfigurationRoot
                        .GetValue<string>(
                            (Environment.IsProduction() || Environment.IsStaging())
                                ? $"ConnectionStrings:{Environment.EnvironmentName}"
                                : $"ConnectionStrings:{EnvironmentHelper.MachineName}");

                // Database Connection String is Simple Type so it still exist in SystemConfig object
                Core.SystemConfigs.DatabaseConnectionString = databaseConnectionString;
                Core.SystemConfigs.Serilog = ConfigurationRoot.GetSection<Core.ConfigModels.SerilogConfigModel>(nameof(Core.SystemConfigs.Serilog));
                Core.SystemConfigs.Developers = ConfigurationRoot.GetSection<Core.ConfigModels.DevelopersConfigModel>(nameof(Core.SystemConfigs.Developers));
                Core.SystemConfigs.Server = ConfigurationRoot.GetSection<Core.ConfigModels.ServerConfigModel>(nameof(Core.SystemConfigs.Server));
                Core.SystemConfigs.IdentityServer = ConfigurationRoot.GetSection<Core.ConfigModels.IdentityServerConfigModel>(nameof(Core.SystemConfigs.IdentityServer));
                Core.SystemConfigs.Redis = ConfigurationRoot.GetSection<Core.ConfigModels.RedisConfigModel>(nameof(Core.SystemConfigs.Redis));
                Core.SystemConfigs.Elastic = ConfigurationRoot.GetSection<Core.ConfigModels.ElasticConfigModel>(nameof(Core.SystemConfigs.Elastic));
            }
        }
    }
}