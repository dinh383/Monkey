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
using Monkey.Model.Models.User;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Threading.Tasks;

namespace Monkey.Service.Facade
{
    [PerRequestDependency(ServiceType = typeof(IAuthenticationService))]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationBusiness _authenticationBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IClientBusiness _clientBusiness;

        public AuthenticationService(IAuthenticationBusiness authenticationBusiness, IUserBusiness userBusiness, IClientBusiness clientBusiness)
        {
            _authenticationBusiness = authenticationBusiness;
            _userBusiness = userBusiness;
            _clientBusiness = clientBusiness;
        }

        public async Task<AccessTokenModel> GetTokenAsync(RequestTokenModel model)
        {
            _clientBusiness.CheckExist(model.ClientId, model.ClientSecret);
            int clientId = await _clientBusiness.GetIdAsync(model.ClientId, model.ClientSecret).ConfigureAwait(true);

            var accessTokenExpire = TimeSpan.FromMinutes(30);
            AccessTokenModel accessToken = null;
            LoggedUserModel loggedUser;

            if (model.GrantType == GrantType.Password)
            {
                _userBusiness.CheckExists(model.UserName);
                _userBusiness.CheckActives(model.UserName);

                // Sing in and get user info
                loggedUser = await _authenticationBusiness.SignInAsync(model.UserName, model.Password).ConfigureAwait(true);
                loggedUser.ClientId = clientId;
                loggedUser.ClientNo = model.ClientId;

                // Save refresh token after sign in success
                var refreshToken = Guid.NewGuid().ToString("N");
                await _authenticationBusiness.SaveRefreshTokenAsync(loggedUser.Id, clientId, refreshToken, null).ConfigureAwait(true);

                // Generate access token
                accessToken = TokenHelper.GenerateAccessToken(model.ClientId, loggedUser.GlobalId, accessTokenExpire, refreshToken);
            }
            else if (model.GrantType == GrantType.RefreshToken)
            {
                // Verify
                _authenticationBusiness.CheckValidRefreshToken(model.RefreshToken, clientId);

                // Get info
                loggedUser = await _authenticationBusiness.GetUserInfoAsync(model.RefreshToken).ConfigureAwait(true);
                loggedUser.ClientId = clientId;
                loggedUser.ClientNo = model.ClientId;

                // Generate access token
                accessToken = TokenHelper.GenerateAccessToken(model.ClientId, loggedUser.GlobalId, accessTokenExpire, model.RefreshToken);
            }
            return accessToken;
        }

        public Task<LoggedUserModel> GetUserInfoAsync(string globalId)
        {
            _userBusiness.CheckExists(globalId);
            return _authenticationBusiness.GetUserInfoByGlobalIdAsync(globalId);
        }

        public Task ExpireAllRefreshTokenAsync(int id)
        {
            _userBusiness.CheckExists(id);
            return _authenticationBusiness.ExpireAllRefreshTokenAsync(id);
        }
    }
}