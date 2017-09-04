#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> GrantType.cs </Name>
//         <Created> 04/09/17 10:36:45 PM </Created>
//         <Key> 356817d4-f882-4f7e-af37-94c94bf1252d </Key>
//     </File>
//     <Summary>
//         GrantType.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Monkey.Authentication
{
    public enum GrantType
    {
        [Display(Name = "implicit")]
        [Description("Client is native app or SPA (client side web app)." +
                     " No require for client_id, client_secret and not have refresh token." +
                     " App => dialog or redirect to identity web page => login success and redirect back to app/SPA with access_token in query string")]
        Implicit,

        [Display(Name = "authorization_code")]
        [Description("Client is web server." +
                     " Require client_id, client_secret and have refresh token." +
                     " Browser (User-agent)" +
                     " => dialog or redirect to identity server (no need client_id and client_secret)" +
                     " => login success and redirect back to web page, get authorization token via query string" +
                     " => send authorization token to web server to exchange/get (includes client_id and client_secret) access_token from identity server.")]
        AuthorizationCode,

        [Display(Name = "password")]
        [Description("Client is trusted app." +
                     " Require client_id, username (user-agent), password (user-agent)." +
                     " Not have dialog or redirect to identity server web page.")]
        ResourceOwnerPassword,

        [Display(Name = "client_credentials")]
        [Description("Client is server/service not interact with end-user." +
                     " Require client_id, client_secret. Not have refresh token." +
                     " Not have dialog or redirect to identity server web page." +
                     " Client => client_id, client_secret send to Identity Server => get access_token")]
        ClientCredentials,

        [Display(Name = "refresh_token")]
        RefreshToken
    }
}