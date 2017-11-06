using Microsoft.AspNetCore.Http;
using Monkey.Auth.Helpers;
using Monkey.Auth.Interfaces;
using Monkey.Core;
using Monkey.Core.Constants.Auth;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Puppy.DependencyInjection;
using System.Threading.Tasks;

namespace Monkey.Auth.Filters
{
    public class LoggedInUserBinder
    {
        public static void BindLoggedInUser(HttpContext httpContext)
        {
            IAuthenticationService authenticationService = AuthConfig.AppBuilder.Resolve<IAuthenticationService>();

            // Access Token found in Header.
            if (TokenHelper.IsHaveAccessTokenInHeader(httpContext.Request))
            {
                string token = TokenHelper.GetValidAndNotExpireAccessToken(httpContext.Request.Headers);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return;
                }

                // Update Current Logged In User in both Static Global variable and HttpContext
                LoggedInUser.Current = authenticationService.GetLoggedInUserAsync(token).Result;
                httpContext.User = TokenHelper.GetClaimsPrincipal(token);

                return;
            }

            // Cookie case
            try
            {
                // Sign In by Cookie
                var accessTokenModel = authenticationService.SignInCookieAsync(httpContext.Request.Cookies).Result;

                if (accessTokenModel == null)
                {
                    return;
                }

                // If current cookie access token is valid but expire, then auto refresh.
                if (TokenHelper.IsExpire(accessTokenModel.AccessToken))
                {
                    RequestTokenModel requestTokenModel = new RequestTokenModel
                    {
                        GrantType = GrantType.RefreshToken,
                        RefreshToken = accessTokenModel.RefreshToken
                    };

                    // Sign In by Request Token Model
                    accessTokenModel = authenticationService.SignInAsync(requestTokenModel).Result;

                    httpContext.Response.OnStarting(state =>
                    {
                        var onResponseHttpContext = (HttpContext)state;

                        TokenHelper.SetAccessTokenInCookie(onResponseHttpContext.Response.Cookies, accessTokenModel);

                        return Task.CompletedTask;
                    }, httpContext);
                }
            }
            catch (MonkeyException ex)
            {
                if (ex.Code == ErrorCode.UserNotExist)
                {
                    return;
                }

                throw;
            }
        }
    }
}