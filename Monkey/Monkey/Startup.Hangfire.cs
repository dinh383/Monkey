using Hangfire;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Monkey
{
    public partial class Startup
    {
        public static class Hangfire
        {
            private static readonly string HangfireDashboard = ConfigurationRoot.GetValue<string>("Developers:HangfireDashboard");

            public static void Service(IServiceCollection services)
            {
                services.AddHangfire(config => config.UseSqlServerStorage(ConfigurationRoot.GetConnectionString(Environment.EnvironmentName)));

                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                JobHelper.SetSerializerSettings(settings);
            }

            public static void Middleware(IApplicationBuilder app)
            {
                app.UseHangfireDashboard(HangfireDashboard, new DashboardOptions
                {
                    Authorization = new[] { new CustomAuthorizeFilter() }
                });
                app.UseHangfireServer();
            }

            public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
            {
                public bool Authorize([NotNull] DashboardContext context)
                {
                    var httpContext = context.GetHttpContext();
                    return IsDeveloperCanAccess(httpContext);
                }
            }
        }
    }
}