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
                var taskGetLoggedInUser = authenticationService.GetLoggedInUserAsync(token);
                taskGetLoggedInUser.Wait();

                LoggedInUser.Current = taskGetLoggedInUser.Result;
                httpContext.User = TokenHelper.GetClaimsPrincipal(token);

                return;
            }

            // Cookie case
            try
            {
                // Sign In by Cookie
                var taskSignInCookie = authenticationService.SignInCookieAsync(httpContext.Request.Cookies);
                taskSignInCookie.Wait();

                var accessTokenModel = taskSignInCookie.Result;

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
                    var taskSignIn = authenticationService.SignInAsync(requestTokenModel);
                    taskSignIn.Wait();

                    accessTokenModel = taskSignIn.Result;

                    httpContext.Response.OnStarting(state =>
                    {
                        var onResponseHttpContext = (HttpContext)state;

                        TokenHelper.SetAccessTokenInCookie(onResponseHttpContext.Response.Cookies, accessTokenModel);

                        return Task.FromResult(0);
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