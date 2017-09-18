#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Facade </Project>
//     <File>
//         <Name> AuthenticationService.cs </Name>
//         <Created> 13/09/17 10:48:09 PM </Created>
//         <Key> d4fd8acd-6479-492d-bcae-5e0293049dbe </Key>
//     </File>
//     <Summary>
//         AuthenticationService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Http;
using Monkey.Auth;
using Monkey.Auth.Helpers;
using Monkey.Auth.Interfaces;
using Monkey.Business;
using Monkey.Core;
using Monkey.Core.Constants.Auth;
using Monkey.Core.Models.Auth;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Threading.Tasks;
using HttpContext = System.Web.HttpContext;

namespace Monkey.Service.Facade
{
    [PerRequestDependency(ServiceType = typeof(IAuthenticationService))]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationBusiness _authenticationBusiness;
        private readonly IClientBusiness _clientBusiness;

        public AuthenticationService(IAuthenticationBusiness authenticationBusiness, IClientBusiness clientBusiness)
        {
            _authenticationBusiness = authenticationBusiness;
            _clientBusiness = clientBusiness;
        }

        /// <inheritdoc />
        public async Task<AccessTokenModel> SignInAsync(RequestTokenModel model)
        {
            _clientBusiness.CheckExist(model.ClientId, model.ClientSecret);

            _clientBusiness.CheckBanned(model.ClientId, model.ClientSecret);

            int clientId = await _clientBusiness.GetIdAsync(model.ClientId, model.ClientSecret).ConfigureAwait(true);

            var accessTokenExpire = TimeSpan.FromMinutes(30);

            AccessTokenModel accessTokenModel = null;

            if (model.GrantType == GrantType.Password)
            {
                _authenticationBusiness.CheckExistsByUserName(model.UserName);
                _authenticationBusiness.CheckValidSignIn(model.UserName, model.Password);

                LoggedInUser.Current = _authenticationBusiness.SignIn(clientId, model.UserName, model.Password, out string refreshToken);

                // Generate access token
                accessTokenModel = TokenHelper.GenerateAccessToken(model.ClientId, LoggedInUser.Current.Subject, accessTokenExpire, refreshToken);

                HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);
            }
            else if (model.GrantType == GrantType.RefreshToken)
            {
                _authenticationBusiness.CheckValidRefreshToken(clientId, model.RefreshToken);

                LoggedInUser.Current = await _authenticationBusiness.GetLoggedInUserByRefreshTokenAsync(model.RefreshToken).ConfigureAwait(true);

                // Generate access token
                accessTokenModel = TokenHelper.GenerateAccessToken(model.ClientId, LoggedInUser.Current.Subject, accessTokenExpire, model.RefreshToken);

                HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);
            }

            return accessTokenModel;
        }

        /// <inheritdoc />
        public async Task SignInCookieAsync(IResponseCookies cookies, AccessTokenModel accessTokenModel)
        {
            TokenHelper.SetAccessTokenInCookie(cookies, accessTokenModel);

            LoggedInUser.Current = await GetLoggedInUserAsync(accessTokenModel.AccessToken).ConfigureAwait(true);

            HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);
        }

        /// <inheritdoc />
        public async Task<AccessTokenModel> SignInCookieAsync(IRequestCookieCollection cookies)
        {
            var accessTokenModel = TokenHelper.GetAccessTokenInCookie(cookies);

            if (accessTokenModel == null)
            {
                return null;
            }

            string accessTokenClientId = TokenHelper.GetAccessTokenClientId(accessTokenModel.AccessToken);

            if (!TokenHelper.IsValidToken(accessTokenModel.AccessToken) || accessTokenClientId != AuthConfig.SystemClientId)
            {
                return null;
            }

            LoggedInUser.Current = await GetLoggedInUserAsync(accessTokenModel.AccessToken).ConfigureAwait(true);

            HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);

            return accessTokenModel;
        }

        /// <inheritdoc />
        public Task SignOutCookieAsync(IResponseCookies cookies)
        {
            TokenHelper.RemoveAccessTokenInCookie(cookies);

            LoggedInUser.Current = null;

            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                HttpContext.Current.User = null;
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<LoggedInUserModel> GetLoggedInUserAsync(string accessToken)
        {
            string subject = TokenHelper.GetAccessTokenSubject(accessToken);
            _authenticationBusiness.CheckExistsBySubject(subject);
            return _authenticationBusiness.GetLoggedInUserBySubjectAsync(subject);
        }

        /// <inheritdoc />
        public Task ExpireAllRefreshTokenAsync(string accessToken)
        {
            string subject = TokenHelper.GetAccessTokenSubject(accessToken);
            _authenticationBusiness.CheckExistsBySubject(subject);
            return _authenticationBusiness.ExpireAllRefreshTokenAsync(subject);
        }
    }
}