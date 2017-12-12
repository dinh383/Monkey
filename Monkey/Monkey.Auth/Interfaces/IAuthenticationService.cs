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
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Auth.Interfaces
{
    public interface IAuthenticationService
    {
        #region Get

        /// <summary>
        ///     Get <see cref="LoggedInUserModel" /> from valid access token 
        /// </summary>
        /// <param name="accessToken">      </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LoggedInUserModel> GetLoggedInUserAsync(string accessToken, CancellationToken cancellationToken = default);

        #endregion

        #region Sign In and Sign Out

        /// <summary>
        ///     Get access token and get <see cref="LoggedInUserModel" /> data for
        ///     LoggedInUser.Current, ClaimsPrincipal for HttpContext.User
        /// </summary>
        /// <param name="httpContext">      </param>
        /// <param name="model">            </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AccessTokenModel> SignInAsync(HttpContext httpContext, RequestTokenModel model, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Set Cookie in Response and Get <see cref="LoggedInUserModel" /> data for
        ///     LoggedInUser.Current, ClaimsPrincipal for HttpContext.User
        /// </summary>
        /// <param name="httpContext">      </param>
        /// <param name="accessTokenModel"> </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SignInCookieAsync(HttpContext httpContext, AccessTokenModel accessTokenModel, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get valid (not check expire) access token and get
        ///     <see cref="LoggedInUserModel" /> data for LoggedInUser.Current, ClaimsPrincipal
        ///     for HttpContext.User
        /// </summary>
        /// <param name="httpContext">      </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AccessTokenModel> SignInCookieAsync(HttpContext httpContext, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Remove Cookie value and Set null for LoggedInUser.Current, null for HttpContext.User 
        /// </summary>
        /// <param name="httpContext">      </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SignOutCookieAsync(HttpContext httpContext, CancellationToken cancellationToken = default);

        #endregion

        #region Refresh Token

        /// <summary>
        ///     Expire all refresh token by valid access token, this method make user need Sign In again
        /// </summary>
        /// <param name="accessToken">      </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExpireAllRefreshTokenAsync(string accessToken, CancellationToken cancellationToken = default);

        #endregion

        #region Get Password

        Task SendConfirmEmailOrSetPasswordAsync(string email, CancellationToken cancellationToken = default);

        #endregion

        #region Set Password

        Task ConfirmEmailAsync(SetPasswordModel model, CancellationToken cancellationToken = default);

        Task SetPasswordAsync(SetPasswordModel model, CancellationToken cancellationToken = default);

        Task ChangePasswordAsync(ChangePasswordModel model, CancellationToken cancellationToken = default);

        #endregion

        #region Validation

        bool IsExpireOrInvalidConfirmEmailToken(string token, CancellationToken cancellationToken = default);

        bool IsExpireOrInvalidSetPasswordToken(string token, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Check current password of logged in user 
        /// </summary>
        /// <param name="currentPassword">  </param>
        /// <param name="cancellationToken"></param>
        void CheckCurrentPassword(string currentPassword, CancellationToken cancellationToken = default);

        #endregion
    }
}