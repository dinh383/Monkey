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
using Monkey.Business.Auth;
using Monkey.Core;
using Monkey.Core.Constants.Auth;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Threading.Tasks;
using HttpContext = System.Web.HttpContext;

namespace Monkey.Service.Facade.Auth
{
    [PerRequestDependency(ServiceType = typeof(IAuthenticationService))]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationBusiness _authenticationBusiness;
        private readonly IClientBusiness _clientBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IEmailBusiness _emailBusiness;

        public AuthenticationService(IAuthenticationBusiness authenticationBusiness, IClientBusiness clientBusiness, IUserBusiness userBusiness, IEmailBusiness emailBusiness)
        {
            _authenticationBusiness = authenticationBusiness;
            _clientBusiness = clientBusiness;
            _userBusiness = userBusiness;
            _emailBusiness = emailBusiness;
        }

        public async Task<AccessTokenModel> SignInAsync(RequestTokenModel model)
        {
            int? clientId = null;

            // Case Client Id and Client Secret is null is system sign in => by pass validate
            if (!string.IsNullOrWhiteSpace(model.ClientId) || !string.IsNullOrWhiteSpace(model.ClientSecret))
            {
                _clientBusiness.CheckExist(model.ClientId, model.ClientSecret);

                _clientBusiness.CheckBanned(model.ClientId, model.ClientSecret);

                clientId = await _clientBusiness.GetIdAsync(model.ClientId, model.ClientSecret).ConfigureAwait(true);
            }

            AccessTokenModel accessTokenModel = null;

            if (model.GrantType == GrantType.Password)
            {
                _userBusiness.CheckExistByUserName(model.UserName);

                _authenticationBusiness.CheckValidSignIn(model.UserName, model.Password);

                LoggedInUser.Current = _authenticationBusiness.SignIn(model.UserName, model.Password, out string refreshToken, clientId);

                // Generate access token
                accessTokenModel = TokenHelper.GenerateAccessToken(model.ClientId, LoggedInUser.Current.Subject, AuthConfig.AccessTokenExpireIn, refreshToken);

                HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);
            }
            else if (model.GrantType == GrantType.RefreshToken)
            {
                _authenticationBusiness.CheckValidRefreshToken(model.RefreshToken, clientId);

                LoggedInUser.Current = await _authenticationBusiness.GetLoggedInUserByRefreshTokenAsync(model.RefreshToken).ConfigureAwait(true);

                // Generate access token
                accessTokenModel = TokenHelper.GenerateAccessToken(model.ClientId, LoggedInUser.Current.Subject, AuthConfig.AccessTokenExpireIn, model.RefreshToken);

                HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);
            }

            return accessTokenModel;
        }

        public async Task SignInCookieAsync(IResponseCookies cookies, AccessTokenModel accessTokenModel)
        {
            TokenHelper.SetAccessTokenInCookie(cookies, accessTokenModel);

            LoggedInUser.Current = await GetLoggedInUserAsync(accessTokenModel.AccessToken).ConfigureAwait(true);

            HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);
        }

        public async Task<AccessTokenModel> SignInCookieAsync(IRequestCookieCollection cookies)
        {
            var accessTokenModel = TokenHelper.GetAccessTokenInCookie(cookies);

            if (accessTokenModel == null)
            {
                return null;
            }

            string accessTokenClientId = TokenHelper.GetAccessTokenClientId(accessTokenModel.AccessToken);

            if (!TokenHelper.IsValidToken(accessTokenModel.AccessToken) || !string.IsNullOrWhiteSpace(accessTokenClientId))
            {
                return null;
            }

            LoggedInUser.Current = await GetLoggedInUserAsync(accessTokenModel.AccessToken).ConfigureAwait(true);

            HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);

            return accessTokenModel;
        }

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

        public Task<LoggedInUserModel> GetLoggedInUserAsync(string accessToken)
        {
            string subject = TokenHelper.GetAccessTokenSubject(accessToken);
            _userBusiness.CheckExistsBySubject(subject);
            return _authenticationBusiness.GetLoggedInUserBySubjectAsync(subject);
        }

        public Task ExpireAllRefreshTokenAsync(string accessToken)
        {
            string subject = TokenHelper.GetAccessTokenSubject(accessToken);
            _userBusiness.CheckExistsBySubject(subject);
            return _authenticationBusiness.ExpireAllRefreshTokenAsync(subject);
        }

        public async Task SendConfirmEmailOrSetPasswordAsync(string email)
        {
            _userBusiness.CheckExistByEmail(email);

            UserModel userModel = await _userBusiness.GetByEmailAsync(email).ConfigureAwait(true);

            if (userModel.ActiveTime == null)
            {
                string token = _authenticationBusiness.GenerateTokenConfirmEmail(userModel.Subject, userModel.Email, out TimeSpan expireIn);
                _emailBusiness.SendActiveAccount(token, email, expireIn);
            }
            else
            {
                string token = _authenticationBusiness.GenerateTokenSetPassword(userModel.Subject, userModel.Email, out TimeSpan expireIn);
                _emailBusiness.SendSetPassword(token, email, expireIn);
            }
        }

        public async Task ConfirmEmailAsync(SetPasswordModel model)
        {
            if (IsExpireOrInvalidConfirmEmailToken(model.Token))
            {
                throw new MonkeyException(ErrorCode.UserConfirmEmailTokenExpireOrInvalid);
            }

            var subject = TokenHelper.GetAccessTokenData<string>(model.Token, nameof(EmailTokenModel.Subject));

            var email = TokenHelper.GetAccessTokenData<string>(model.Token, nameof(EmailTokenModel.Email));

            _userBusiness.CheckExistsBySubject(subject);

            await _authenticationBusiness.ConfirmEmailAsync(subject, email, model.Password).ConfigureAwait(true);

            _authenticationBusiness.ExpireTokenConfirmEmail(model.Token);
        }

        public bool IsExpireOrInvalidConfirmEmailToken(string token)
        {
            bool isExpireOrInvalidToken = TokenHelper.IsExpireOrInvalidToken(token);

            if (isExpireOrInvalidToken)
            {
                return true;
            }

            return _authenticationBusiness.IsExpireOrInvalidConfirmEmailToken(token);
        }

        public async Task SetPasswordAsync(SetPasswordModel model)
        {
            if (IsExpireOrInvalidSetPasswordToken(model.Token))
            {
                throw new MonkeyException(ErrorCode.UserSetPasswordTokenExpireOrInvalid);
            }

            var subject = TokenHelper.GetAccessTokenData<string>(model.Token, nameof(EmailTokenModel.Subject));

            _userBusiness.CheckExistsBySubject(subject);

            await _authenticationBusiness.SetPasswordAsync(subject, model.Password).ConfigureAwait(true);
            _authenticationBusiness.ExpireTokenSetPassword(model.Token);
        }

        public bool IsExpireOrInvalidSetPasswordToken(string token)
        {
            bool isExpireOrInvalidToken = TokenHelper.IsExpireOrInvalidToken(token);

            if (isExpireOrInvalidToken)
            {
                return true;
            }

            return _authenticationBusiness.IsExpireOrInvalidSetPasswordToken(token);
        }
    }
}