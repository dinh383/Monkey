#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> HangfireExtensions.cs </Name>
//         <Created> 31/07/17 10:41:56 PM </Created>
//         <Key> 56a4974a-dc9a-4f33-8f89-816f4b1ff71d </Key>
//     </File>
//     <Summary>
//         HangfireExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Hangfire;
using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Monkey.Areas.Developers;
using Monkey.Core;

namespace Monkey.Extensions
{
    public static class HangfireExtensions
    {
        /// <summary>
        ///     [Background Job] Hangfire 
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddHangfireMonkey(this IServiceCollection services)
        {
            services.AddHangfire(config => config.UseSqlServerStorage(SystemConfigs.DatabaseConnectionString));

            JobHelper.SetSerializerSettings(Core.Constants.JsonSerializerSettings);

            return services;
        }

        /// <summary>
        ///     [Background Job] Hangfire Use dashboard and server 
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseHangfireMonkey(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard(SystemConfigs.Developers.HangfireDashboardUrl, new DashboardOptions
            {
                Authorization = new[] { new CustomAuthorizeFilter() }
            });
            app.UseHangfireServer();

            return app;
        }

        public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize([NotNull] DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return DeveloperHelper.IsCanAccess(httpContext);
            }
        }
    }
}