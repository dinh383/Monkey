using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Monkey.Authentication;
using Monkey.Core;
using Monkey.Core.Constants;
using Monkey.Core.Models.User;
using Monkey.Service;
using Newtonsoft.Json;
using Puppy.DependencyInjection;
using System.Security.Claims;
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
                if (context.Request.Cookies.TryGetValue(Authentication.Constants.AccessTokenCookieName, out string cookieValue))
                {
                    try
                    {
                        AccessTokenModel accessToken = JsonConvert.DeserializeObject<AccessTokenModel>(cookieValue);
                        string accessTokenClientId = TokenHelper.GetAccessTokenClientId(accessToken.AccessToken);

                        if (TokenHelper.IsValidToken(accessToken.AccessToken, out ClaimsPrincipal claimsPrincipal) && accessTokenClientId == SystemConfigs.Identity.ClientId)
                        {
                            context.User = claimsPrincipal;

                            // If current cookie access token is expire but valid, then auto refresh.
                            // This logic just for WEB Cookie
                            if (TokenHelper.IsExpireOrInvalidToken(accessToken.AccessToken))
                            {
                                var authenticationService = _appBuilder.Resolve<IAuthenticationService>();
                                RequestTokenModel requestTokenModel = new RequestTokenModel
                                {
                                    ClientId = SystemConfigs.Identity.ClientId,
                                    ClientSecret = SystemConfigs.Identity.ClientSecret,
                                    GrantType = GrantType.RefreshToken,
                                    RefreshToken = accessToken.RefreshToken
                                };

                                var newAccessToken = await authenticationService.GetTokenAsync(requestTokenModel).ConfigureAwait(true);

                                context.Response.OnStarting(state =>
                                {
                                    var httpContext = (HttpContext)state;
                                    httpContext.Response.Cookies.Append(Authentication.Constants.AccessTokenCookieName, JsonConvert.SerializeObject(newAccessToken));
                                    return Task.CompletedTask;
                                }, context);
                            }
                        }
                    }
                    catch
                    {
                        // nothing
                    }
                }
                await _next.Invoke(context).ConfigureAwait(true);
            }
        }
    }
}