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
using Puppy.Core.StringUtils;
using Puppy.Core.TypeUtils;
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
        /// <summary>
        ///     Access token cookie name depend on Assembly and Secret Key to make difference between systems.
        /// </summary>
        private static readonly string AccessTokenCookieName = $"{typeof(TokenHelper).GetAssembly().FullName}|{nameof(AccessTokenCookieName)}".Encrypt(AuthenticationConfig.SecretKey);

        /// <summary>
        ///     Refresh token cookie name depend on Assembly and Secret Key to make difference
        ///     between systems.
        /// </summary>
        private static readonly string RefreshTokenCookieName = $"{typeof(TokenHelper).GetAssembly().FullName}|{nameof(RefreshTokenCookieName)}".Encrypt(AuthenticationConfig.SecretKey);

        #region Generate

        public static AccessTokenModel GenerateAccessToken(string clientId, string subject, TimeSpan expiresSpan, string refreshToken, string issuer = null)
        {
            var dateTimeUtcNow = DateTimeOffset.UtcNow;
            double authTime = dateTimeUtcNow.GetEpochTime();

            var accessToken = new AccessTokenModel
            {
                ExpireIn = expiresSpan.TotalSeconds,
                ExpireOn = dateTimeUtcNow.AddSeconds(expiresSpan.TotalSeconds),
                RefreshToken = refreshToken,
                TokenType = Constants.AuthenticationTokenType
            };

            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {"client_id", clientId},
                {JwtRegisteredClaimNames.Sub, subject},
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

        #region Cookie

        public static void SetAccessTokenToCookie(IResponseCookies cookies, AccessTokenModel accessToken)
        {
            // Access Token
            string accessTokenEncrypted = accessToken.AccessToken.Encrypt(AuthenticationConfig.SecretKey);
            cookies.Append(AccessTokenCookieName, accessTokenEncrypted);

            // Refresh Token
            string refreshTokenEncrypted = accessToken.RefreshToken.Encrypt(AuthenticationConfig.SecretKey);
            cookies.Append(RefreshTokenCookieName, refreshTokenEncrypted);
        }

        public static AccessTokenModel GetAccessTokenFromCookie(IRequestCookieCollection cookies)
        {
            var accessToken = GetCookieValue(cookies, AccessTokenCookieName);

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return null;
            }

            var refreshToken = GetCookieValue(cookies, RefreshTokenCookieName);

            AccessTokenModel accessTokenModel = new AccessTokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = Constants.AuthenticationTokenType,
                ExpireOn = GetAccessTokenExpireOn(accessToken)
            };

            accessTokenModel.ExpireIn = accessTokenModel.ExpireOn?.Subtract(DateTimeOffset.UtcNow).TotalSeconds ?? -1;

            return accessTokenModel;
        }

        private static string GetCookieValue(IRequestCookieCollection cookies, string key)
        {
            if (!cookies.TryGetValue(key, out var cookieValue))
            {
                return null;
            }

            if (!cookieValue.TryDecrypt(AuthenticationConfig.SecretKey, out var accessToken))
            {
                return null;
            }

            return accessToken;
        }

        #endregion

        #region Validation

        public static bool IsValidToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                handler.ValidateToken(token, AuthenticationConfig.TokenValidationParameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsExpire(string token)
        {
            DateTimeOffset? expireOn = GetAccessTokenExpireOn(token);
            return expireOn != null && expireOn <= DateTimeOffset.UtcNow;
        }

        public static bool IsExpireOrInvalidToken(string token)
        {
            return !IsValidToken(token) || IsExpire(token);
        }

        #endregion

        #region Get Data

        public static ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                return handler.ValidateToken(token, AuthenticationConfig.TokenValidationParameters, out _);
            }
            catch
            {
                return null;
            }
        }

        public static string GetAuthenticationToken(HttpRequest request)
        {
            var authenticationHeader = request.Headers[HeaderKey.Authorization].ToString();
            var token = authenticationHeader.Replace(Constants.AuthenticationTokenType, string.Empty)?.Trim();

            if (string.IsNullOrWhiteSpace(token))
            {
                if (request.Cookies.TryGetValue(AccessTokenCookieName, out string cookieValue))
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

        public static DateTimeOffset? GetAccessTokenExpireOn(string token)
        {
            double? expireOn = GetAccessTokenData<double?>(token, JwtRegisteredClaimNames.Exp);
            return expireOn == null ? (DateTimeOffset?)null : DateTimeHelper.GetDateTimeFromEpoch(expireOn.Value);
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
            if (!IsValidToken(token))
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