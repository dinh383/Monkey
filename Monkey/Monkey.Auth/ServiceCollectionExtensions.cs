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
using Microsoft.Extensions.Primitives;
using Monkey.Auth.Helpers;
using Monkey.Auth.Interfaces;
using Monkey.Core;
using Monkey.Core.Constants.Auth;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Puppy.DependencyInjection;
using Puppy.Web.Middlewares;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Auth
{
    public static class ServiceCollectionExtensions
    {
        private static IApplicationBuilder _appBuilder;
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

            ChangeToken.OnChange(_configuration.GetReloadToken, () =>
            {
                // Re-Build the config for DataTable
                _configuration.BuildConfig(_configSection);
            });

            app.UseHttpContextAccessor();

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

                // Sign In to the context
                IAuthenticationService authenticationService = _appBuilder.Resolve<IAuthenticationService>();

                try
                {
                    var accessTokenModel = await authenticationService.SignInCookieAsync(context.Request.Cookies).ConfigureAwait(true);

                    if (accessTokenModel == null)
                    {
                        await _next.Invoke(context).ConfigureAwait(true);
                        return;
                    }

                    // If current cookie access token is valid but expire, then auto refresh. This
                    // logic just for WEB Cookie
                    if (TokenHelper.IsExpire(accessTokenModel.AccessToken))
                    {
                        RequestTokenModel requestTokenModel = new RequestTokenModel
                        {
                            GrantType = GrantType.RefreshToken,
                            RefreshToken = accessTokenModel.RefreshToken
                        };

                        var newAccessTokenModel = await authenticationService.SignInAsync(requestTokenModel).ConfigureAwait(true);

                        context.Response.OnStarting(state =>
                        {
                            var httpContext = (HttpContext)state;

                            TokenHelper.SetAccessTokenInCookie(httpContext.Response.Cookies, newAccessTokenModel);

                            return Task.CompletedTask;
                        }, context);
                    }
                }
                catch (MonkeyException ex)
                {
                    if (ex.Code == ErrorCode.UserNotExist)
                    {
                        await _next.Invoke(context).ConfigureAwait(true);
                        return;
                    }

                    throw;
                }

                await _next.Invoke(context).ConfigureAwait(true);
            }
        }

        /// <summary>
        ///     Get data for LoggedInUser.Current by HTTP Request Header, JWT Middleware already put
        ///     value for HttpContext.User (Http Context Identity)
        /// </summary>
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

                string token = TokenHelper.GetValidAndNotExpireAccessToken(context.Request.Headers);

                if (string.IsNullOrWhiteSpace(token))
                {
                    await _next.Invoke(context).ConfigureAwait(true);
                    return;
                }

                // Only set value for LoggedInUser.Current, JWT Middleware already put value for
                // HttpContext.User data.

                LoggedInUser.Current = await authenticationService.GetLoggedInUserAsync(token).ConfigureAwait(true);

                await _next.Invoke(context).ConfigureAwait(true);
            }
        }

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