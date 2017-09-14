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

using Newtonsoft.Json;
using System;

namespace Monkey.Model.Models.User
{
    public class AccessTokenModel
    {
        public string TokenType { get; set; }

        /// <summary>
        ///     Expire on UTC 
        /// </summary>
        public DateTimeOffset? ExpireOn { get; set; }

        public string AccessToken { get; set; }

        [JsonIgnore]
        public string ClientId { get; set; }

        [JsonIgnore]
        public string Subject { get; set; }

        /// <summary>
        ///     Lifetime of token in seconds 
        /// </summary>
        public double ExpireIn { get; set; }

        public string RefreshToken { get; set; }
    }
}