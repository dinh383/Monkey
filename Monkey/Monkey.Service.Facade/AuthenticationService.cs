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

using Monkey.Authentication;
using Monkey.Business;
using Monkey.Core.Constants;
using Monkey.Core.Models.User;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Threading.Tasks;

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

        public async Task<AccessTokenModel> GetTokenAsync(RequestTokenModel model)
        {
            _clientBusiness.CheckExist(model.ClientId, model.ClientSecret);

            _clientBusiness.CheckBanned(model.ClientId, model.ClientSecret);

            int clientId = await _clientBusiness.GetIdAsync(model.ClientId, model.ClientSecret).ConfigureAwait(true);

            var accessTokenExpire = TimeSpan.FromMinutes(30);
            AccessTokenModel accessToken = null;

            if (model.GrantType == GrantType.Password)
            {
                _authenticationBusiness.CheckExistsByUserName(model.UserName);
                _authenticationBusiness.CheckValidSignIn(model.UserName, model.Password);

                var loggedInUser = _authenticationBusiness.SignIn(clientId, model.UserName, model.Password, out string refreshToken);

                // Generate access token
                accessToken = TokenHelper.GenerateAccessToken(model.ClientId, loggedInUser.Subject, accessTokenExpire, refreshToken);
            }
            else if (model.GrantType == GrantType.RefreshToken)
            {
                // Verify
                _authenticationBusiness.CheckValidRefreshToken(clientId, model.RefreshToken);

                var loggedInUser = await _authenticationBusiness.GetByRefreshTokenAsync(model.RefreshToken).ConfigureAwait(true);

                // Generate access token
                accessToken = TokenHelper.GenerateAccessToken(model.ClientId, loggedInUser.Subject, accessTokenExpire, model.RefreshToken);
            }
            return accessToken;
        }

        public Task<LoggedInUserModel> GetUserInfoAsync(string subject)
        {
            _authenticationBusiness.CheckExistsBySubject(subject);
            return _authenticationBusiness.GetBySubjectAsync(subject);
        }

        public Task ExpireAllRefreshTokenAsync(string subject)
        {
            _authenticationBusiness.CheckExistsBySubject(subject);
            return _authenticationBusiness.ExpireAllRefreshTokenAsync(subject);
        }
    }
}