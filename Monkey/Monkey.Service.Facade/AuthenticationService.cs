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

        public AuthenticationService(IAuthenticationBusiness authenticationBusiness, IUserBusiness userBusiness)
        {
            _authenticationBusiness = authenticationBusiness;
            _userBusiness = userBusiness;
        }

        public async Task<AccessTokenModel> SignInAsync(RequestTokenModel model)
        {
            var accessTokenExpire = TimeSpan.FromMinutes(30);

            AccessTokenModel accessToken = null;
            LoggedUserModel loggedUser;

            if (model.GrantType == GrantType.ResourceOwnerPassword)
            {
                _userBusiness.CheckExists(model.Username);

                loggedUser = await _authenticationBusiness.SignInAsync(model.Username, model.Password).ConfigureAwait(true);

                var refreshToken = Guid.NewGuid().ToString("N");
                await _authenticationBusiness.SaveRefreshTokenAsync(loggedUser.Id, refreshToken, null).ConfigureAwait(true);
                accessToken = TokenHelper.GenerateAccessToken(loggedUser, accessTokenExpire, refreshToken);
            }
            else if (model.GrantType == GrantType.RefreshToken)
            {
                _authenticationBusiness.CheckValidRefreshToken(model.RefreshToken);

                loggedUser = await _authenticationBusiness.GetUserInfoAsync(model.RefreshToken).ConfigureAwait(true);

                accessToken = TokenHelper.GenerateAccessToken(loggedUser, accessTokenExpire, model.RefreshToken);
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