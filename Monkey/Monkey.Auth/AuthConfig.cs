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

using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Puppy.Core.StringUtils;
using System;
using System.Text;

namespace Monkey.Auth
{
    public static class AuthConfig
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

        public static TimeSpan AccessTokenExpireIn { get; set; }

        internal static SymmetricSecurityKey SecurityKey { get; private set; }

        internal static SigningCredentials SigningCredentials { get; private set; }

        internal static IApplicationBuilder AppBuilder { get; set; }

        internal static TokenValidationParameters TokenValidationParameters => new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKey,
            ValidateLifetime = false,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateActor = false,
            ClockSkew = TimeSpan.Zero
        };
    }
}