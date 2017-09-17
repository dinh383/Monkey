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

using Monkey.Authentication.Config;
using Monkey.Authentication.Interfaces;
using Monkey.Business;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Threading.Tasks;

namespace Monkey.Authentication.Services
{
    [PerRequestDependency(ServiceType = typeof(IAuthenticationService))]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationBusiness _authenticationBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IClientBusiness _clientBusiness;
        private readonly TimeSpan _accessTokenExpire = TimeSpan.FromMinutes(30);

        public AuthenticationService(IAuthenticationBusiness authenticationBusiness, IUserBusiness userBusiness, IClientBusiness clientBusiness)
        {
            _authenticationBusiness = authenticationBusiness;
            _userBusiness = userBusiness;
            _clientBusiness = clientBusiness;
        }

        public async Task<IAccessTokenModel> GetTokenAsync(IRequestTokenModel model)
        {
            _clientBusiness.CheckExist(model.ClientId, model.ClientSecret);
            var clientId = await _clientBusiness.GetIdAsync(model.ClientId, model.ClientSecret).ConfigureAwait(true);

            IAccessTokenModel accessToken = null;

            if (model.GrantType == GrantType.Password)
            {
                _userBusiness.CheckExists(model.UserName);
                _userBusiness.CheckActives(model.UserName);
                _authenticationBusiness.CheckValidSignInAsync(model.UserName, model.Password, AuthenticationConfig.SecretKey);

                // Sign In
                string subject = _authenticationBusiness.SignIn(model.UserName, clientId, out string refreshToken);

                // Generate access token
                accessToken = TokenHelper.GenerateAccessToken(model.ClientId, subject, _accessTokenExpire, refreshToken);
            }
            else if (model.GrantType == GrantType.RefreshToken)
            {
                // Verify
                _authenticationBusiness.CheckValidRefreshToken(clientId, model.RefreshToken);

                var subject = await _userBusiness.GetUserSubjectByRefreshTokenAsync(model.RefreshToken).ConfigureAwait(true);

                // Generate access token
                accessToken = TokenHelper.GenerateAccessToken(model.ClientId, subject, _accessTokenExpire, model.RefreshToken);
            }
            return accessToken;
        }

        public Task ExpireAllRefreshTokenAsync(string subject)
        {
            _userBusiness.CheckExists(subject);
            _authenticationBusiness.ExpireAllRefreshToken(subject);

            return Task.CompletedTask;
        }
    }
}