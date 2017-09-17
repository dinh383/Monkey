#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> PasswordHelper.cs </Name>
//         <Created> 17/09/17 10:22:19 PM </Created>
//         <Key> 771c8a3b-e56c-452e-96c9-2215816a1c81 </Key>
//     </File>
//     <Summary>
//         PasswordHelper.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;
using Puppy.Core.StringUtils;

namespace Monkey.Authentication.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password, DateTimeOffset hashTime)
        {
            var passwordSalt = hashTime.ToString("O") + AuthConfig.SecretKey;
            var passwordHash = password.HashPassword(passwordSalt);
            return passwordHash;
        }
    }
}