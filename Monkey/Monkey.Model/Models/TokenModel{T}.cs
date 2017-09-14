#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> TokenModel.cs </Name>
//         <Created> 14/09/17 11:01:09 AM </Created>
//         <Key> 01770867-e4dc-4840-9b18-f7cde51fdd3a </Key>
//     </File>
//     <Summary>
//         TokenModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Monkey.Model.Models
{
    public class TokenModel<T>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TokenType TokenType { get; set; } = TokenType.Bearer;

        /// <summary>
        ///     Expire on UTC 
        /// </summary>
        public DateTimeOffset? ExpireOn { get; set; }

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

    public enum TokenType
    {
        Bearer
    }
}