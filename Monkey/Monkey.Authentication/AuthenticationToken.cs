#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> AuthenticationToken.cs </Name>
//         <Created> 03/09/17 2:44:22 PM </Created>
//         <Key> be00166a-7aff-4e8a-abae-dc5b0625ee4a </Key>
//     </File>
//     <Summary>
//         AuthenticationToken.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;

namespace Monkey.Authentication
{
    public class AuthenticationToken
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTimeOffset? ExpireOn { get; set; }

        public DateTimeOffset CreatedTime { get; set; }

        public string Type { get; set; }
    }
}