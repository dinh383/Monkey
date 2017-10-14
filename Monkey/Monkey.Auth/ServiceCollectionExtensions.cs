#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ServiceCollectionExtensions.cs </Name>
//         <Created> 03/09/17 1:20:49 PM </Created>
//         <Key> 59754712-afa6-42cc-860d-9dc7c49002c5 </Key>
//     </File>
//     <Summary>
//         ServiceCollectionExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Auth.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Puppy.Core.ServiceCollectionUtils;
using Puppy.Web.Middlewares;
using System;
using System.Linq;

namespace Monkey.Auth
{
    public static class ServiceCollectionExtensions
    {
        private static IConfiguration _configuration;
        private static string _configSection;

        /// <summary>
        ///     [Authentication] Json Web Token + Cookie 
        /// </summary>
        /// <param name="services">     </param>
        /// <param name="configuration"></param>
        /// <param name="configSection"></param>
        /// <returns></returns>
        public static IServiceCollection AddHybridAuth(this IServiceCollection services, IConfiguration configuration, string configSection = Constants.Constant.DefaultConfigSection)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _configSection = configSection;
            configuration.BuildConfig(configSection);

            services.AddScopedIfNotExist<ApiAuthActionFilter>();
            services.AddScopedIfNotExist<MvcAuthActionFilter>();
            services.AddScopedIfNotExist<BindingLoggedInUserFilter>();

            // Add System.HttpContext.Current
            services.AddHttpContextAccessor();

            return services;
        }

        /// <summary>
        ///     [Authentication] Json Web Token + Cookie 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        /// <remarks>
        ///     The global config for HybridAuth from appsettings.json will auto reload when you
        ///     change config. Be careful when you change the SecretKey, it affect to existing
        ///     PasswordHash and Token
        /// </remarks>
        public static IApplicationBuilder UseHybridAuth(this IApplicationBuilder app)
        {
            _configuration.BuildConfig(_configSection);

            // Use System.HttpContext.Current
            app.UseHttpContextAccessor();

            ChangeToken.OnChange(_configuration.GetReloadToken, () =>
            {
                // Re-Build the config for DataTable
                _configuration.BuildConfig(_configSection);
            });

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = AuthConfig.TokenValidationParameters
            });

            AuthConfig.AppBuilder = app;

            return app;
        }

        #region Middlewares

        #endregion

        public static void BuildConfig(this IConfiguration configuration, string configSection = Constants.Constant.DefaultConfigSection)
        {
            var isHaveConfig = configuration.GetChildren().Any(x => x.Key == configSection);

            if (isHaveConfig)
            {
                AuthConfig.SecretKey = configuration.GetValue($"{configSection}:{nameof(AuthConfig.SecretKey)}", AuthConfig.SecretKey);
                AuthConfig.AccessTokenExpireIn = configuration.GetValue($"{configSection}:{nameof(AuthConfig.AccessTokenExpireIn)}", AuthConfig.AccessTokenExpireIn);
            }
        }
    }
}