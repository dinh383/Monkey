#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> RequestTokenModel.cs </Name>
//         <Created> 04/09/17 10:34:44 PM </Created>
//         <Key> 1754adc4-14bb-4d8f-8b79-5fa70f617f07 </Key>
//     </File>
//     <Summary>
//         RequestTokenModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Monkey.Authentication
{
    public abstract class RequestTokenModel
    {
        [JsonProperty(Constants.Oauth.GrantType)]
        [JsonConverter(typeof(StringEnumConverter))]
        public GrantType GrantType { get; set; }

        [JsonProperty(Constants.Oauth.ClientId)]
        public string ClientId { get; set; }

        [JsonProperty(Constants.Oauth.RedirectUri)]
        public string RedirectUri { get; set; }

        [JsonProperty(Constants.Oauth.ResponseType)]
        public string ResponseType { get; } = Constants.Oauth.AuthorizationCodeResponseType;

        [JsonProperty(Constants.Oauth.Scopes)]
        public List<string> Scopes { get; set; }

        [JsonProperty(Constants.Oauth.Code)]
        public string Code { get; set; }

        [JsonProperty(Constants.Oauth.Username)]
        public string Username { get; set; }

        [JsonProperty(Constants.Oauth.Password)]
        public string Password { get; set; }

        [JsonProperty(Constants.Oauth.RefreshToken)]
        public string RefreshToken { get; set; }
    }
}