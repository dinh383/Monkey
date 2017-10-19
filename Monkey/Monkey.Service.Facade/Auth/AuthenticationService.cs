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

using Monkey.Auth;
using Monkey.Auth.Helpers;
using Monkey.Auth.Interfaces;
using Monkey.Business;
using Monkey.Business.Auth;
using Monkey.Business.User;
using Monkey.Core;
using Monkey.Core.Constants.Auth;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Monkey.Core.Models.User;
using Microsoft.AspNetCore.Http;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Threading;
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

        public async Task<AccessTokenModel> SignInAsync(RequestTokenModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            int? clientId = null;

            // Case Client Id and Client Secret is null is system sign in => by pass validate
            if (!string.IsNullOrWhiteSpace(model.ClientId) || !string.IsNullOrWhiteSpace(model.ClientSecret))
            {
                _clientBusiness.CheckExist(model.ClientId, model.ClientSecret);

                _clientBusiness.CheckBanned(model.ClientId, model.ClientSecret);

                clientId = await _clientBusiness.GetIdAsync(model.ClientId, model.ClientSecret, cancellationToken).ConfigureAwait(true);
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

                LoggedInUser.Current = await _authenticationBusiness.GetLoggedInUserByRefreshTokenAsync(model.RefreshToken, cancellationToken).ConfigureAwait(true);

                // Generate access token
                accessTokenModel = TokenHelper.GenerateAccessToken(model.ClientId, LoggedInUser.Current.Subject, AuthConfig.AccessTokenExpireIn, model.RefreshToken);

                HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);
            }

            return accessTokenModel;
        }

        public async Task SignInCookieAsync(IResponseCookies cookies, AccessTokenModel accessTokenModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            TokenHelper.SetAccessTokenInCookie(cookies, accessTokenModel);

            LoggedInUser.Current = await GetLoggedInUserAsync(accessTokenModel.AccessToken, cancellationToken).ConfigureAwait(true);

            HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);
        }

        public async Task<AccessTokenModel> SignInCookieAsync(IRequestCookieCollection cookies, CancellationToken cancellationToken = default(CancellationToken))
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

            LoggedInUser.Current = await GetLoggedInUserAsync(accessTokenModel.AccessToken, cancellationToken).ConfigureAwait(true);

            HttpContext.Current.User = TokenHelper.GetClaimsPrincipal(accessTokenModel.AccessToken);

            return accessTokenModel;
        }

        public Task SignOutCookieAsync(IResponseCookies cookies, CancellationToken cancellationToken = default(CancellationToken))
        {
            TokenHelper.RemoveAccessTokenInCookie(cookies);

            LoggedInUser.Current = null;

            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                HttpContext.Current.User = null;
            }

            return Task.CompletedTask;
        }

        public Task<LoggedInUserModel> GetLoggedInUserAsync(string accessToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            string subject = TokenHelper.GetAccessTokenSubject(accessToken);
            _userBusiness.CheckExistsBySubject(subject);
            return _authenticationBusiness.GetLoggedInUserBySubjectAsync(subject, cancellationToken);
        }

        public Task ExpireAllRefreshTokenAsync(string accessToken, CancellationToken cancellationToken = default(CancellationToken))
        {
            string subject = TokenHelper.GetAccessTokenSubject(accessToken);
            _userBusiness.CheckExistsBySubject(subject);
            return _authenticationBusiness.ExpireAllRefreshTokenAsync(subject, cancellationToken);
        }

        public async Task SendConfirmEmailOrSetPasswordAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            _userBusiness.CheckExistByEmail(email);

            UserModel userModel = await _userBusiness.GetByEmailAsync(email, cancellationToken).ConfigureAwait(true);

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

        public async Task ConfirmEmailAsync(SetPasswordModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsExpireOrInvalidConfirmEmailToken(model.Token))
            {
                throw new MonkeyException(ErrorCode.UserConfirmEmailTokenExpireOrInvalid);
            }

            var subject = TokenHelper.GetAccessTokenData<string>(model.Token, nameof(EmailTokenModel.Subject));

            var email = TokenHelper.GetAccessTokenData<string>(model.Token, nameof(EmailTokenModel.Email));

            _userBusiness.CheckExistsBySubject(subject);

            await _authenticationBusiness.ConfirmEmailAsync(subject, email, model.Password, cancellationToken).ConfigureAwait(true);

            _authenticationBusiness.ExpireTokenConfirmEmail(model.Token);
        }

        public bool IsExpireOrInvalidConfirmEmailToken(string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool isExpireOrInvalidToken = TokenHelper.IsExpireOrInvalidToken(token);

            if (isExpireOrInvalidToken)
            {
                return true;
            }

            return _authenticationBusiness.IsExpireOrInvalidConfirmEmailToken(token);
        }

        public async Task SetPasswordAsync(SetPasswordModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (IsExpireOrInvalidSetPasswordToken(model.Token))
            {
                throw new MonkeyException(ErrorCode.UserSetPasswordTokenExpireOrInvalid);
            }

            var subject = TokenHelper.GetAccessTokenData<string>(model.Token, nameof(EmailTokenModel.Subject));

            _userBusiness.CheckExistsBySubject(subject);

            await _authenticationBusiness.SetPasswordAsync(subject, model.Password, cancellationToken).ConfigureAwait(true);
            _authenticationBusiness.ExpireTokenSetPassword(model.Token);
        }

        public bool IsExpireOrInvalidSetPasswordToken(string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool isExpireOrInvalidToken = TokenHelper.IsExpireOrInvalidToken(token);

            if (isExpireOrInvalidToken)
            {
                return true;
            }

            return _authenticationBusiness.IsExpireOrInvalidSetPasswordToken(token);
        }

        public void CheckCurrentPassword(string currentPassword, CancellationToken cancellationToken = default(CancellationToken))
        {
            _authenticationBusiness.CheckCurrentPassword(currentPassword);
        }

        public Task ChangePasswordAsync(ChangePasswordModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            _authenticationBusiness.CheckCurrentPassword(model.CurrentPassword);
            return _authenticationBusiness.SetPasswordAsync(LoggedInUser.Current.Subject, model.Password, cancellationToken);
        }
    }
}