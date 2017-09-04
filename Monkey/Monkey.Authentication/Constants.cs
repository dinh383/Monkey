#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> Constants.cs </Name>
//         <Created> 03/09/17 1:35:14 PM </Created>
//         <Key> a74f3b14-277a-4029-84f6-c90e1733cdba </Key>
//     </File>
//     <Summary>
//         Constants.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

namespace Monkey.Authentication
{
    public static class Constants
    {
        public const string DefaultConfigSection = "Authentication";

        public static class Oauth
        {
            public const string ClientId = "client_id";
            public const string ClientSecret = "client_secret";
            public const string Scopes = "scopes";
            public const string GrantType = "grant_type";
            public const string RedirectUri = "redirect_uri";
            public const string ResponseType = "response_type";
            public const string Code = "code";
            public const string Username = "username";
            public const string Password = "password";
            public const string RefreshToken = "refresh_token";
            public const string Id = "id";
            public const string AccessToken = "access_token";
            public const string TokenType = "token_type";
            public const string ExpireIn = "expire_in";
            public const string ExpireOn = "expire_on";
            public const string IssuedAt = "issued_at";
            public const string AuthorizationCodeResponseType = "code";
            public const string Bearer = "Bearer";
        }
    }
}