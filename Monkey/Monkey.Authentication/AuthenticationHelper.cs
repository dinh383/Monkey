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
        public static string GenerateToken<T>(T data, DateTime? expireOn, DateTime? issuedAt = null, DateTime? notBefore = null, string issuer = null)
        {
            var identityClaims = GetClaimsIdentity(data);

            var dateTimeUtcNow = DateTime.UtcNow;

            var handler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                SigningCredentials = AuthenticationConfig.SigningCredentials,
                Expires = expireOn ?? dateTimeUtcNow,
                NotBefore = notBefore ?? dateTimeUtcNow,
                IssuedAt = issuedAt ?? dateTimeUtcNow,
                Issuer = issuer
            });

            var token = handler.WriteToken(securityToken);

            return token;
        }

        public static AccessTokenModel GenerateAccessToken<T>(T data, string issuer = null)
        {
            var token = new AccessTokenModel
            {
                IssuedAt = DateTimeOffset.UtcNow,
                ExpireIn = AuthenticationConfig.ExpiresSpan.TotalMilliseconds,
            };

            token.ExpireOn = token.IssuedAt.AddMilliseconds(token.ExpireIn).DateTime;

            token.AccessToken = GenerateToken(data, token.ExpireOn?.DateTime, token.IssuedAt.DateTime, token.IssuedAt.DateTime, issuer);

            token.RefreshToken = GenerateRefreshAccessToken(token.AccessToken, out var refreshTokenId, issuer);

            token.RefreshTokenId = refreshTokenId;

            return token;
        }

        private static string GenerateRefreshAccessToken(string accessTokenJwt, out string refreshTokenId, string issuer = null)
        {
            var refreshToken = new RefreshTokenModel(accessTokenJwt);

            string refreshTokenJwt = GenerateToken(refreshToken, null, refreshToken.IssuedAt.DateTime, refreshToken.IssuedAt.DateTime, issuer);

            refreshTokenId = refreshToken.Id;

            return refreshTokenJwt;
        }

        private static ClaimsIdentity GetClaimsIdentity<T>(T data)
        {
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
            return identity;
        }
    }
}