#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> TokenHelper.cs </Name>
//         <Created> 03/09/17 1:57:19 PM </Created>
//         <Key> af183ae1-5300-49fc-bba2-16b8cc084799 </Key>
//     </File>
//     <Summary>
//         TokenHelper.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Monkey.Authentication
{
    public static class TokenHelper
    {
        public static string GenerateToken<T>(TokenModel<T> tokenData) where T : class
        {
            var identityClaims = GetClaimsIdentity(tokenData);

            var handler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                SigningCredentials = AuthenticationConfig.SigningCredentials,
                Expires = tokenData.ExpireOn?.UtcDateTime,
                IssuedAt = tokenData.IssuedAt.UtcDateTime,
                NotBefore = tokenData.IssuedAt.UtcDateTime,
                Issuer = tokenData.Issuer
            });

            var token = handler.WriteToken(securityToken);

            return token;
        }

        public static TokenModel<T> GetTokenData<T>(HttpRequest request) where T : class
        {
            var authenticationHeader = request.Headers["Authorization"].ToString();
            var token = authenticationHeader.Replace(Constants.TokenType.Bearer, string.Empty)?.Trim();
            var tokenData = GetTokenData<T>(token);
            return tokenData;
        }

        public static TokenModel<T> GetTokenData<T>(string token) where T : class
        {
            if (!TryReadTokenPayload(token, out var tokenPayload))
            {
                return null;
            }

            T data = null;

            if (tokenPayload.TryGetValue(nameof(TokenModel<T>.Data), out var dataObj))
            {
                data = dataObj == null ? null : JsonConvert.DeserializeObject<T>(dataObj.ToString(), Puppy.Core.Constants.StandardFormat.JsonSerializerSettings);
            }

            TokenModel<string> tokenDataStr = JsonConvert.DeserializeObject<TokenModel<string>>(tokenPayload.SerializeToJson());

            TokenModel<T> tokenData = new TokenModel<T>(data)
            {
                Issuer = tokenDataStr.Issuer,
                IssuedAt = tokenDataStr.IssuedAt,
                TokenType = tokenDataStr.TokenType,
                ExpireOn = tokenDataStr.ExpireOn
            };
            return tokenData;
        }

        public static ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            IsValidToken(token, out var claimsPrincipal);
            return claimsPrincipal;
        }

        private static bool TryReadTokenPayload(string token, out JwtPayload tokenPayload)
        {
            if (!IsValidToken(token, out _))
            {
                tokenPayload = null;
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            tokenPayload = JwtPayload.Base64UrlDeserialize(jwtToken.EncodedPayload);
            return tokenPayload?.Keys.Any() == true;
        }

        public static bool IsValidToken(string token, out ClaimsPrincipal claimsPrincipal)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                claimsPrincipal = handler.ValidateToken(token, AuthenticationConfig.TokenValidationParameters, out _);
                return true;
            }
            catch
            {
                claimsPrincipal = null;
                return false;
            }
        }

        public static bool IsExpireOrInvalidToken(string token)
        {
            if (!IsValidToken(token, out _)) return true;

            DateTimeOffset dateTimeNow = DateTimeOffset.UtcNow;

            TokenModel<object> tokenData = GetTokenData<object>(token);

            if (tokenData.ExpireOn == null || dateTimeNow > tokenData.ExpireOn)
            {
                return false;
            }
            return true;
        }

        public static AccessTokenModel GenerateAccessToken<T>(T data, TimeSpan expiresSpan, string refreshToken, string issuer = null) where T : class
        {
            var dateTimeUtcNow = DateTime.UtcNow;

            var accessToken = new AccessTokenModel
            {
                ExpireIn = expiresSpan.TotalSeconds,
                ExpireOn = dateTimeUtcNow.AddSeconds(expiresSpan.TotalSeconds),
                RefreshToken = refreshToken,
                TokenType = Constants.TokenType.Bearer
            };

            var tokenData = new TokenModel<T>(data)
            {
                IssuedAt = dateTimeUtcNow,
                ExpireOn = accessToken.ExpireOn,
                Issuer = issuer,
                TokenType = accessToken.TokenType
            };

            accessToken.AccessToken = GenerateToken(tokenData);

            return accessToken;
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