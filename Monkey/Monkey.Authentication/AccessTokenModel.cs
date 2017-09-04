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

namespace Monkey.Authentication
{
    /// <summary>
    ///     Token model follow OAUTH 2.0 Standard 
    /// </summary>
    public class AccessTokenModel
    {
        [JsonProperty(Constants.Oauth.Id, Order = -1)]
        public string Id { get; set; } = Guid.NewGuid().ToString("D");

        [JsonProperty(Constants.Oauth.AccessToken, Order = 1)]
        public string AccessToken { get; set; }

        [JsonProperty(Constants.Oauth.TokenType, Order = 2)]
        public string TokenType { get; set; } = Constants.Oauth.Bearer;

        [JsonProperty(Constants.Oauth.ExpireIn, Order = 3)]
        public double ExpireIn { get; set; }

        [JsonProperty(Constants.Oauth.ExpireOn, Order = 4)]
        public DateTimeOffset? ExpireOn { get; set; }

        [JsonProperty(Constants.Oauth.IssuedAt, Order = 5)]
        public DateTimeOffset IssuedAt { get; set; } = DateTimeOffset.UtcNow;

        [JsonIgnore]
        public string RefreshTokenId { get; set; }

        [JsonProperty(Constants.Oauth.RefreshToken, Order = 9999)]
        public string RefreshToken { get; set; }
    }
}