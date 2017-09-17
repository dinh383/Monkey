#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> AccessTokenModel.cs </Name>
//         <Created> 03/09/17 2:44:22 PM </Created>
//         <Key> be00166a-7aff-4e8a-abae-dc5b0625ee4a </Key>
//     </File>
//     <Summary>
//         AccessTokenModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Authentication.Interfaces;
using System;

namespace Monkey.Authentication.Models
{
    public class AccessTokenModel : IAccessTokenModel
    {
        /// <inheritdoc />
        public string TokenType { get; set; }

        /// <inheritdoc />
        public DateTimeOffset? ExpireOn { get; set; }

        /// <inheritdoc />
        public string AccessToken { get; set; }

        /// <inheritdoc />
        public double ExpireIn { get; set; }

        /// <inheritdoc />
        public string RefreshToken { get; set; }
    }
}