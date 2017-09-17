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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monkey.Core;
using Monkey.Core.Constants;
using Monkey.Core.Models.User;
using Puppy.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Monkey.Authentication.Helpers;
using Monkey.Authentication.Interfaces;

namespace Monkey.Authentication
{
    public static class ServiceCollectionExtensions
    {
        private static IApplicationBuilder _appBuilder;

        /// <summary>
        ///     [Authentication] Json Web Token + Cookie 
        /// </summary>
        /// <param name="services">     </param>
        /// <param name="configuration"></param>
        /// <param name="configSection"></param>
        /// <returns></returns>
        public static IServiceCollection AddHybridAuth(this IServiceCollection services, IConfiguration configuration, string configSection = Constants.DefaultConfigSection)
        {
            configuration.BuildConfig(configSection);
            return services;
        }

        public static void BuildConfig(this IConfiguration configuration, string configSection = Constants.DefaultConfigSection)
        {
            var isHaveConfig = configuration.GetChildren().Any(x => x.Key == configSection);

            if (isHaveConfig)
            {
                AuthConfig.SecretKey = configuration.GetValue($"{configSection}:{nameof(AuthConfig.SecretKey)}", AuthConfig.SecretKey);
                AuthConfig.SystemClientId = configuration.GetValue($"{configSection}:{nameof(AuthConfig.SystemClientId)}", AuthConfig.SystemClientId);
                AuthConfig.SystemClientSecret = configuration.GetValue($"{configSection}:{nameof(AuthConfig.SystemClientSecret)}", AuthConfig.SystemClientSecret);
            }
        }

        /// <summary>
        ///     [Authentication] Json Web Token + Cookie 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHybridAuth(this IApplicationBuilder app)
        {
            app.UseMiddleware<CookieAuthMiddleware>();

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = AuthConfig.TokenValidationParameters
            });

            app.UseMiddleware<LoggedInUserMiddleware>();

            _appBuilder = app;

            return app;
        }

        #region Middlewares

        public class CookieAuthMiddleware
        {
            private readonly RequestDelegate _next;

            public CookieAuthMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                // SKIP if Access Token found in Header - not check valid or not.
                if (TokenHelper.IsHaveAccessTokenInHeader(context.Request))
                {
                    await _next.Invoke(context).ConfigureAwait(true);
                    return;
                }

                var accessTokenModel = TokenHelper.GetAccessTokenFromCookie(context.Request.Cookies);

                if (accessTokenModel == null)
                {
                    await _next.Invoke(context).ConfigureAwait(true);
                    return;
                }

                string accessTokenClientId = TokenHelper.GetAccessTokenClientId(accessTokenModel.AccessToken);

                if (!TokenHelper.IsValidToken(accessTokenModel.AccessToken) || accessTokenClientId != AuthConfig.SystemClientId)
                {
                    await _next.Invoke(context).ConfigureAwait(true);
                    return;
                }

                // Sign In to the context
                context.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);

                // If current cookie access token is valid but expire, then auto refresh. This logic
                // just for WEB Cookie
                if (TokenHelper.IsExpire(accessTokenModel.AccessToken))
                {
                    var authenticationService = _appBuilder.Resolve<IAuthenticationService>();

                    RequestTokenModel requestTokenModel = new RequestTokenModel
                    {
                        ClientId = AuthConfig.SystemClientId,
                        ClientSecret = AuthConfig.SystemClientSecret,
                        GrantType = GrantType.RefreshToken,
                        RefreshToken = accessTokenModel.RefreshToken
                    };

                    var newAccessTokenModel = await authenticationService.GetTokenAsync(requestTokenModel).ConfigureAwait(true);

                    context.Response.OnStarting(state =>
                    {
                        var httpContext = (HttpContext)state;

                        TokenHelper.SetAccessTokenToCookie(httpContext.Response.Cookies, newAccessTokenModel);

                        return Task.CompletedTask;
                    }, context);
                }

                await _next.Invoke(context).ConfigureAwait(true);
            }
        }

        public class LoggedInUserMiddleware
        {
            private readonly RequestDelegate _next;

            public LoggedInUserMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                IAuthenticationService authenticationService = _appBuilder.Resolve<IAuthenticationService>();

                string token = TokenHelper.GetAccessToken(context.Request);

                if (string.IsNullOrWhiteSpace(token))
                {
                    await _next.Invoke(context).ConfigureAwait(true);
                    return;
                }

                if (TokenHelper.IsExpireOrInvalidToken(token))
                {
                    await _next.Invoke(context).ConfigureAwait(true);
                    return;
                }

                string userGlobalId = TokenHelper.GetAccessTokenSubject(token);

                if (string.IsNullOrWhiteSpace(userGlobalId))
                {
                    await _next.Invoke(context).ConfigureAwait(true);
                    return;
                }

                LoggedInUser.Current = await authenticationService.GetUserInfoAsync(userGlobalId).ConfigureAwait(true);

                LoggedInUser.Current.ClientSubject = TokenHelper.GetAccessTokenClientId(token);

                await _next.Invoke(context).ConfigureAwait(true);
            }
        }

        #endregion
    }
}