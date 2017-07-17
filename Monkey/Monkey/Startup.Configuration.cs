using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Monkey
{
    public partial class Startup
    {
        public static class Configuration
        {
            public static void Service(IServiceCollection services)
            {
                services.AddSingleton(ConfigurationRoot);
                services.AddSingleton(Environment);
            }

            public static void Middleware(IApplicationBuilder app, ILoggerFactory loggerFactory)
            {
                // [Important] The order of middleware very important for request and response
                // handle! Don't mad it !!!

                // Currently, ASPNETCORE have a BUG hit twice when change appsetting.json from
                // 20/03/17 (see more: https://github.com/aspnet/Configuration/issues/624)
                ChangeToken.OnChange(ConfigurationRoot.GetReloadToken, () => loggerFactory.CreateLogger<Startup>().LogWarning("Configuration Changed"));
            }
        }
    }
}