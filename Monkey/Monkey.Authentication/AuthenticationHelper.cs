#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> AuthenticationHelper.cs </Name>
//         <Created> 03/09/17 1:57:19 PM </Created>
//         <Key> af183ae1-5300-49fc-bba2-16b8cc084799 </Key>
//     </File>
//     <Summary>
//         AuthenticationHelper.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Monkey.Authentication
{
    public static class AuthenticationHelper
    {
        public static string GenerateToken<T>(T data)
        {
            var dateTimeutcNow = DateTime.UtcNow;

            // Generate Dictionary from T
            Dictionary<string, string> dictionary;

            var dataStr = data as string;
            if (!string.IsNullOrWhiteSpace(dataStr))
            {
                dictionary = new Dictionary<string, string>
                {
                    {nameof(data), dataStr}
                };
            }
            else
            {
                dictionary = Puppy.Core.DictionaryUtils.DictionaryHelper.ToDictionary(data);
            }

            // Generate Identity
            ClaimsIdentity identity = new ClaimsIdentity();
            foreach (var key in dictionary.Keys)
            {
                identity.AddClaim(new Claim(key, dictionary[key]));
            }

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = identity,
                SigningCredentials = AuthenticationConfig.SigningCredentials,
                Expires = dateTimeutcNow.AddTicks(AuthenticationConfig.ExpiresSpan.Ticks),
                NotBefore = dateTimeutcNow,
                IssuedAt = dateTimeutcNow
            });

            return handler.WriteToken(securityToken);
        }
    }
}