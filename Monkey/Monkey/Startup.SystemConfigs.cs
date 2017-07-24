using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Puppy.Core.ConfigUtils;
using Puppy.Core.EnvironmentUtils;

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
                BuildSystemConfig(ConfigurationRoot);
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
                    BuildSystemConfig(ConfigurationRoot);

                    loggerFactory.CreateLogger<Startup>().LogWarning($"{nameof(Core.SystemConfigs)} Changed!");
                });
            }

            public static void BuildSystemConfig(IConfiguration configuration)
            {
                // Database Connection String
                GetDatabaseConnectionStringConfig(configuration);

                GetMvcPathConfig(configuration);

                Core.SystemConfigs.Serilog = configuration.GetSection<Core.ConfigModels.SerilogConfigModel>(nameof(Core.SystemConfigs.Serilog));

                Core.SystemConfigs.Developers = configuration.GetSection<Core.ConfigModels.DevelopersConfigModel>(nameof(Core.SystemConfigs.Developers));

                Core.SystemConfigs.Server = configuration.GetSection<Core.ConfigModels.ServerConfigModel>(nameof(Core.SystemConfigs.Server));

                Core.SystemConfigs.IdentityServer = configuration.GetSection<Core.ConfigModels.IdentityServerConfigModel>(nameof(Core.SystemConfigs.IdentityServer));

                Core.SystemConfigs.Redis = configuration.GetSection<Core.ConfigModels.RedisConfigModel>(nameof(Core.SystemConfigs.Redis));

                Core.SystemConfigs.Elastic = configuration.GetSection<Core.ConfigModels.ElasticConfigModel>(nameof(Core.SystemConfigs.Elastic));
            }

            private static void GetDatabaseConnectionStringConfig(IConfiguration configuration)
            {
                string databaseConnectionString =
                    configuration
                        .GetValue<string>(
                            (EnvironmentHelper.IsProduction() || EnvironmentHelper.IsStaging())
                                ? $"ConnectionStrings:{EnvironmentHelper.Name}"
                                : $"ConnectionStrings:{EnvironmentHelper.MachineName}");

                // Database Connection String is Simple Type so it still exist in SystemConfig object
                Core.SystemConfigs.DatabaseConnectionString = databaseConnectionString;
            }

            private static void GetMvcPathConfig(IConfiguration configuration)
            {
                Core.SystemConfigs.MvcPath = configuration.GetSection<Core.ConfigModels.MvcPathConfigModel>(nameof(Core.SystemConfigs.MvcPath));
            }
        }
    }
}