#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Author> Top </Author>
//     <Project> Monkey.Auth.Domain.Exceptions </Project>
//     <File>
//         <Name> ExceptionCode </Name>
//         <Created> 12/04/2017 09:05:31 AM </Created>
//         <Key> f183ec48-2edb-4c90-b029-6ee78f6b6cdd </Key>
//     </File>
//     <Summary>
//         ExceptionCode
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Core.Exceptions
{
    /// <summary>
    ///     Error code for whole system 
    /// </summary>
    public enum ErrorCode
    {
        // Global
        [Display(Name = "Bad Request", GroupName = "Global")]
        [Description("Bad Request")]
        BadRequest = 400,

        [Display(Name = "Un-Authenticate", GroupName = "Global")]
        [Description("Un-Authenticate")]
        UnAuthenticated = 401,

        [Display(Name = "Forbidden", GroupName = "Global")]
        [Description("Forbidden, this feature for 18+ :))")]
        Unauthorized = 403,

        [Display(Name = "Unknown", GroupName = "Global")]
        [Description("Awesome, You break the system :o. You know what they say, you get what you pay for... The features do not write themselves, you know. Now, just god and you know what happen.")]
        Unknown = 500,

        [Display(Name = "Invalid refresh token", GroupName = "Token")]
        InvalidRefreshToken = 600,

        [Display(Name = "Refresh token is expired", GroupName = "Token")]
        RefreshTokenExpired = 601,

        [Display(Name = "Invalid access token", GroupName = "Token")]
        InvalidAccessToken = 603,

        [Display(Name = "Access token is expired", GroupName = "Token")]
        AccessTokenExpired = 604,

        [Display(Name = "Invalid client, wrong client id or client secret", GroupName = "Client")]
        InvalidClient = 700,

        [Display(Name = "Client is banned", GroupName = "Client")]
        ClientIsBanned = 7001,

        [Display(Name = "User does not exist", GroupName = "User")]
        UserNotExist = 1000,

        [Display(Name = "User is in-active", GroupName = "User")]
        UserInActive = 1001,

        [Display(Name = "User Name does not exist", GroupName = "User")]
        UserNameNotExist = 1002,

        [Display(Name = "User password is wrong", GroupName = "User")]
        UserPasswordIsWrong = 1003,

        [Display(Name = "User is banned", GroupName = "User")]
        UserIsBanned = 1004,
    }
}