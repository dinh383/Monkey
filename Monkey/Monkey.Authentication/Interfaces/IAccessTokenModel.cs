#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> IAccessTokenModel.cs </Name>
//         <Created> 17/09/17 1:43:30 PM </Created>
//         <Key> 33be012b-35bf-49e8-a18f-9dacab977437 </Key>
//     </File>
//     <Summary>
//         IAccessTokenModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;

namespace Monkey.Authentication.Interfaces
{
    public interface IAccessTokenModel
    {
        string TokenType { get; set; }

        /// <summary>
        ///     Expire on UTC 
        /// </summary>
        DateTimeOffset? ExpireOn { get; set; }

        string AccessToken { get; set; }

        /// <summary>
        ///     Lifetime of token in seconds 
        /// </summary>
        double ExpireIn { get; set; }

        string RefreshToken { get; set; }
    }
}