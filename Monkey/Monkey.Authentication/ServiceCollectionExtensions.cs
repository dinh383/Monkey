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

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Monkey.Authentication
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     [Authentication] Json Web Token 
        /// </summary>
        /// <param name="services">     </param>
        /// <param name="configuration"></param>
        /// <param name="configSection"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtBearerAuthen(this IServiceCollection services, IConfiguration configuration, string configSection = Constants.DefaultConfigSection)
        {
            configuration.BuildConfig(configSection);
            return services;
        }

        /// <summary>
        ///     [Authentication] Json Web Token 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseJwtBearerAuthen(this IApplicationBuilder app)
        {
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = AuthenticationConfig.TokenValidationParameters
            });

            return app;
        }

        public static void BuildConfig(this IConfiguration configuration, string configSection = Constants.DefaultConfigSection)
        {
            var isHaveConfig = configuration.GetChildren().Any(x => x.Key == configSection);

            if (isHaveConfig)
            {
                AuthenticationConfig.SecretKey = configuration.GetValue($"{configSection}:{nameof(AuthenticationConfig.SecretKey)}", AuthenticationConfig.SecretKey);
            }
        }
    }
}