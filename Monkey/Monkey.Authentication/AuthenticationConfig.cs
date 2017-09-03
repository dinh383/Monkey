#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> AuthenticationConfig.cs </Name>
//         <Created> 03/09/17 1:32:01 PM </Created>
//         <Key> fe73ec0b-2c02-4442-983c-4cb173eecf54 </Key>
//     </File>
//     <Summary>
//         AuthenticationConfig.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Puppy.Core.StringUtils;

namespace Monkey.Authentication
{
    public static class AuthenticationConfig
    {
        private static string _secretKey;

        public static string SecretKey
        {
            get => _secretKey;
            set
            {
                StringHelper.CheckNullOrWhiteSpace(value);

                _secretKey = value;

                SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
            }
        }

        public static SymmetricSecurityKey SecurityKey { get; private set; }

        public static SigningCredentials SigningCredentials { get; private set; }

        public static string TokenType { get; } = "Bearer";

        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(30);
    }
}