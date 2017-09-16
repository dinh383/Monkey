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
using Monkey.Core.Models.User;
using Newtonsoft.Json;
using Puppy.Core.DateTimeUtils;
using Puppy.Core.ObjectUtils;
using Puppy.Web.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Monkey.Authentication
{
    public static class TokenHelper
    {
        #region Generate

        public static AccessTokenModel GenerateAccessToken(string clientId, string subject, TimeSpan expiresSpan, string refreshToken, string issuer = null)
        {
            var dateTimeUtcNow = DateTimeOffset.UtcNow;
            double authTime = DateTimeHelper.GetEpochTime(dateTimeUtcNow);

            var accessToken = new AccessTokenModel
            {
                ExpireIn = expiresSpan.TotalSeconds,
                ExpireOn = dateTimeUtcNow.AddSeconds(expiresSpan.TotalSeconds),
                RefreshToken = refreshToken,
                TokenType = Constants.AuthenticationTokenType,
                ClientId = clientId,
                Subject = subject
            };

            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"client_id", accessToken.ClientId},
                {JwtRegisteredClaimNames.Sub, accessToken.Subject},
                {JwtRegisteredClaimNames.AuthTime, authTime.ToString(CultureInfo.InvariantCulture)}
            };

            accessToken.AccessToken = GenerateToken(accessToken.ExpireOn?.UtcDateTime, issuer, dictionary);

            return accessToken;
        }

        public static string GenerateToken(DateTime? expireOn, string issuer, Dictionary<string, string> data)
        {
            var handler = new JwtSecurityTokenHandler();

            var utcTimeNow = DateTime.UtcNow;

            ClaimsIdentity claims = new ClaimsIdentity();
            foreach (var key in data.Keys)
            {
                claims.AddClaim(new Claim(key, data[key]));
            }

            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = claims,
                SigningCredentials = AuthenticationConfig.SigningCredentials,
                Expires = expireOn,
                IssuedAt = utcTimeNow,
                NotBefore = utcTimeNow,
                Issuer = issuer
            });

            var token = handler.WriteToken(securityToken);

            return token;
        }

        #endregion

        #region Validation

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

            DateTime utcNow = DateTime.UtcNow;

            double? epochExpireOn = GetAccessTokenData<double?>(token, JwtRegisteredClaimNames.Exp);

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime? expireOn = epochExpireOn == null ? (DateTime?)null : epoch.AddSeconds(epochExpireOn.Value);

            if (expireOn == null || utcNow < expireOn)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Get Data

        public static ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            IsValidToken(token, out var claimsPrincipal);
            return claimsPrincipal;
        }

        public static string GetAuthenticationToken(HttpRequest request)
        {
            var authenticationHeader = request.Headers[HeaderKey.Authorization].ToString();
            var token = authenticationHeader.Replace(Constants.AuthenticationTokenType, string.Empty)?.Trim();

            if (string.IsNullOrWhiteSpace(token))
            {
                if (request.Cookies.TryGetValue(Constants.AccessTokenCookieName, out string cookieValue))
                {
                    AccessTokenModel accessToken = JsonConvert.DeserializeObject<AccessTokenModel>(cookieValue);
                    return accessToken?.AccessToken;
                }
            }

            return token;
        }

        public static string GetAccessTokenSubject(string token)
        {
            return GetAccessTokenData<string>(token, JwtRegisteredClaimNames.Sub);
        }

        public static string GetAccessTokenClientId(string token)
        {
            return GetAccessTokenData<string>(token, Constants.ClientIdKey);
        }

        public static T GetAccessTokenData<T>(string token, string key)
        {
            if (!TryReadTokenPayload(token, out var tokenPayload))
            {
                return default(T);
            }

            tokenPayload.TryGetValue(key, out var data);

            return data.ConvertTo<T>();
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

        #endregion
    }
}