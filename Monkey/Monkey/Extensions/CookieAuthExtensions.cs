using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Monkey.Authentication;
using Monkey.Core.Constants;
using Monkey.Core.Models.User;
using Monkey.Service;
using Puppy.DependencyInjection;
using System.Threading.Tasks;

namespace Monkey.Extensions
{
    public static class CookieAuthExtensions
    {
        private static IApplicationBuilder _appBuilder;

        /// <summary>
        ///     [Authentication] Get Logged In User Info 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCookieAuth(this IApplicationBuilder app)
        {
            app.UseMiddleware<CookieAuthMiddleware>();

            _appBuilder = app;

            return app;
        }

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

                if (!TokenHelper.IsValidToken(accessTokenModel.AccessToken) || accessTokenClientId != AuthenticationConfig.SystemClientId)
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
                        ClientId = AuthenticationConfig.SystemClientId,
                        ClientSecret = AuthenticationConfig.SystemClientSecret,
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
    }
}