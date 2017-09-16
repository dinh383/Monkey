using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Monkey.Authentication;
using Monkey.Core;
using Monkey.Service;
using Puppy.DependencyInjection;
using System.Threading.Tasks;

namespace Monkey.Extensions
{
    public static class LoggedInUserExtensions
    {
        private static IApplicationBuilder _appBuilder;

        /// <summary>
        ///     [Authentication] Get Logged In User Info 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLoggedInUser(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggedInUserMiddleware>();

            _appBuilder = app;

            return app;
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
                string token = TokenHelper.GetAuthenticationToken(context.Request);

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
                LoggedInUser.Current.ClientId = TokenHelper.GetAccessTokenClientId(token);

                await _next.Invoke(context).ConfigureAwait(true);
            }
        }
    }
}