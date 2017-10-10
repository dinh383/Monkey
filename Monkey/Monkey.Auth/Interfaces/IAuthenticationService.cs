#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Interface </Project>
//     <File>
//         <Name> IAuthenticationService.cs </Name>
//         <Created> 13/09/17 10:47:07 PM </Created>
//         <Key> 96a454c5-15eb-425c-810e-fd9f18cf2c54 </Key>
//     </File>
//     <Summary>
//         IAuthenticationService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Http;
using Monkey.Core.Models.Auth;
using System.Threading.Tasks;

namespace Monkey.Auth.Interfaces
{
    public interface IAuthenticationService
    {
        /// <summary>
        ///     Get access token and get <see cref="LoggedInUserModel" /> data for
        ///     LoggedInUser.Current, ClaimsPrincipal for HttpContext.User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AccessTokenModel> SignInAsync(RequestTokenModel model);

        /// <summary>
        ///     Set Cookie in Response and Get <see cref="LoggedInUserModel" /> data for
        ///     LoggedInUser.Current, ClaimsPrincipal for HttpContext.User
        /// </summary>
        /// <param name="cookies">         </param>
        /// <param name="accessTokenModel"></param>
        /// <returns></returns>
        Task SignInCookieAsync(IResponseCookies cookies, AccessTokenModel accessTokenModel);

        /// <summary>
        ///     Get valid (not check expire) access token and get <see cref="LoggedInUserModel" />
        ///     data for LoggedInUser.Current, ClaimsPrincipal for HttpContext.User
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        Task<AccessTokenModel> SignInCookieAsync(IRequestCookieCollection cookies);

        /// <summary>
        ///     Remove Cookie value and Set null for LoggedInUser.Current, null for HttpContext.User 
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        Task SignOutCookieAsync(IResponseCookies cookies);

        /// <summary>
        ///     Get <see cref="LoggedInUserModel" /> from valid access token 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<LoggedInUserModel> GetLoggedInUserAsync(string accessToken);

        /// <summary>
        ///     Expire all refresh token by valid access token, this method make user need Sign In again
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task ExpireAllRefreshTokenAsync(string accessToken);

        Task SendConfirmEmailOrSetPasswordAsync(string email);

        Task ConfirmEmailAsync(SetPasswordModel model);

        bool IsExpireOrInvalidConfirmEmailToken(string token);

        Task SetPasswordAsync(SetPasswordModel model);

        bool IsExpireOrInvalidSetPasswordToken(string token);

        /// <summary>
        ///     Check current password of logged in user 
        /// </summary>
        /// <param name="currentPassword"></param>
        void CheckCurrentPassword(string currentPassword);

        Task ChangePasswordAsync(ChangePasswordModel model);
    }
}