#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> RefreshTokenModel.cs </Name>
//         <Created> 04/09/17 8:22:56 PM </Created>
//         <Key> 37243802-c119-4166-b9d2-98c2433a89b4 </Key>
//     </File>
//     <Summary>
//         RefreshTokenModel.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;

namespace Monkey.Authentication
{
    public class RefreshTokenModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("D");

        public DateTimeOffset IssuedAt { get; set; } = DateTimeOffset.UtcNow;

        public string AccessTokenJwt { get; set; }

        public RefreshTokenModel(string accessTokenJwt)
        {
            AccessTokenJwt = accessTokenJwt;
        }
    }
}