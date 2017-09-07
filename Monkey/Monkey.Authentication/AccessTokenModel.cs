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
    public class TokenBaseModel
    {
        public string TokenType { get; set; } = Constants.TokenType.Bearer;

        /// <summary>
        ///     Expire on UTC 
        /// </summary>
        public DateTimeOffset? ExpireOn { get; set; }
    }

    public class AccessTokenModel : TokenBaseModel
    {
        /// <summary>
        ///     JWT of <see cref="TokenModel{T}" /> 
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        ///     Lifetime of token in seconds 
        /// </summary>
        public double? ExpireIn { get; set; }

        [JsonIgnore]
        public RefreshTokenModel RefreshTokenData { get; set; }

        public string RefreshToken { get; set; }
    }

    public class TokenModel<T> : TokenBaseModel
    {
        public T Data { get; set; }

        /// <summary>
        ///     Issue at UTC 
        /// </summary>
        public DateTimeOffset IssuedAt { get; set; } = DateTimeOffset.UtcNow;

        public string Issuer { get; set; }

        public TokenModel()
        {
        }

        public TokenModel(T data)
        {
            Data = data;
        }
    }
}