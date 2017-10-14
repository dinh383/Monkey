#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> BindingLoggedInUserFilter.cs </Name>
//         <Created> 13/10/17 8:30:53 PM </Created>
//         <Key> 5b9decd3-ded3-471f-8477-d0a0050d923d </Key>
//     </File>
//     <Summary>
//         BindingLoggedInUserFilter.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Auth.Helpers;
using Monkey.Auth.Interfaces;
using Monkey.Core;
using Monkey.Core.Constants.Auth;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Puppy.DependencyInjection;
using System.Threading.Tasks;

namespace Monkey.Auth.Filters
{
    public class BindingLoggedInUserFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            IAuthenticationService authenticationService = AuthConfig.AppBuilder.Resolve<IAuthenticationService>();

            // Access Token found in Header.
            if (TokenHelper.IsHaveAccessTokenInHeader(context.HttpContext.Request))
            {
                string token = TokenHelper.GetValidAndNotExpireAccessToken(context.HttpContext.Request.Headers);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return;
                }

                // Update Current Logged In User in both Static Global variable and HttpContext
                LoggedInUser.Current = authenticationService.GetLoggedInUserAsync(token).Result;
                context.HttpContext.User = TokenHelper.GetClaimsPrincipal(token);

                return;
            }

            // Cookie case
            try
            {
                // Sign In by Cookie
                var accessTokenModel = authenticationService.SignInCookieAsync(context.HttpContext.Request.Cookies).Result;

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

                    context.HttpContext.Response.OnStarting(state =>
                    {
                        var httpContext = (HttpContext)state;

                        TokenHelper.SetAccessTokenInCookie(httpContext.Response.Cookies, accessTokenModel);

                        return Task.CompletedTask;
                    }, context);
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

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Nothing to filter in Action Executed
        }
    }
}