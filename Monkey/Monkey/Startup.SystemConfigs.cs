using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Monkey
{
    public partial class Startup
    {
        public static class SystemConfigs
        {
            public static void Service(IServiceCollection services)
            {
                services.AddSingleton(ConfigurationRoot);
                services.AddSingleton(Environment);
            }

            public static void Middleware(IApplicationBuilder app, ILoggerFactory loggerFactory)
            {
                // Currently, ASPNETCORE have a BUG hit twice when change appsetting.json from
                // 20/03/17 (see more: https://github.com/aspnet/SystemConfigs/issues/624)
                ChangeToken.OnChange(ConfigurationRoot.GetReloadToken, () =>
                {
                    loggerFactory.CreateLogger<Startup>().LogWarning("SystemConfigs Changed");
                });
            }
        }
    }
}