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
using Monkey.Core.Models.Auth;
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

namespace Monkey.Auth.Helpers
{
    public static class TokenHelper
    {
        /// <summary>
        ///     Access token cookie name depend on Assembly and Secret Key to make difference between systems.
        /// </summary>
        private static readonly string AccessTokenCookieName = $"{nameof(AccessTokenCookieName)}|{typeof(TokenHelper).GetAssemblySimpleName()}".Encrypt(AuthConfig.SecretKey);

        /// <summary>
        ///     Refresh token cookie name depend on Assembly and Secret Key to make difference
        ///     between systems.
        /// </summary>
        private static readonly string RefreshTokenCookieName = $"{nameof(RefreshTokenCookieName)}|{typeof(TokenHelper).GetAssemblySimpleName()}".Encrypt(AuthConfig.SecretKey);

        #region Generate

        public static AccessTokenModel GenerateAccessToken(string clientId, string subject, TimeSpan expiresSpan, string refreshToken, string issuer = null)
        {
            clientId = clientId?.Trim() ?? string.Empty;

            var dateTimeUtcNow = DateTimeOffset.UtcNow;

            double authTime = dateTimeUtcNow.GetEpochTime();

            var accessToken = new AccessTokenModel
            {
                ExpireIn = expiresSpan.TotalSeconds,
                ExpireOn = dateTimeUtcNow.AddSeconds(expiresSpan.TotalSeconds),
                RefreshToken = refreshToken,
                TokenType = Constants.Constant.AuthenticationTokenType
            };

            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                {Constants.Constant.ClientIdKey, clientId},
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
                SigningCredentials = AuthConfig.SigningCredentials,
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

        public static void SetAccessTokenInCookie(IResponseCookies cookies, AccessTokenModel accessToken)
        {
            // Access Token
            SetCookieValue(cookies, AccessTokenCookieName, accessToken.AccessToken);

            // Refresh Token
            SetCookieValue(cookies, RefreshTokenCookieName, accessToken.RefreshToken);
        }

        public static void RemoveAccessTokenInCookie(IResponseCookies cookies)
        {
            // Access Token
            cookies.Delete(AccessTokenCookieName);

            // Refresh Token
            cookies.Delete(RefreshTokenCookieName);
        }

        public static AccessTokenModel GetAccessTokenInCookie(IRequestCookieCollection cookies)
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
                TokenType = Constants.Constant.AuthenticationTokenType,
                ExpireOn = GetAccessTokenExpireOn(accessToken)
            };

            accessTokenModel.ExpireIn = accessTokenModel.ExpireOn?.Subtract(DateTimeOffset.UtcNow).TotalSeconds ?? -1;

            return accessTokenModel;
        }

        public static string GetCookieValue(IRequestCookieCollection cookies, string key)
        {
            if (!cookies.TryGetValue(key, out var cookieValue))
            {
                return null;
            }

            if (!cookieValue.TryDecrypt(AuthConfig.SecretKey, out var accessToken))
            {
                return null;
            }

            return accessToken;
        }

        public static void SetCookieValue(IResponseCookies cookies, string key, string value)
        {
            cookies.Append(key, value.Encrypt(AuthConfig.SecretKey), new CookieOptions
            {
                HttpOnly = true,
                Secure = false // allow transmit via http and https
            });
        }

        #endregion

        #region Validation

        public static bool IsValidToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            try
            {
                handler.ValidateToken(token, AuthConfig.TokenValidationParameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsExpire(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return true;
            }

            DateTimeOffset? expireOn = GetAccessTokenExpireOn(token);
            return expireOn != null && expireOn <= DateTimeOffset.UtcNow;
        }

        public static bool IsExpireOrInvalidToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return true;
            }

            return !IsValidToken(token) || IsExpire(token);
        }

        /// <summary>
        ///     Check Authorization in Header have value or not, this method not check Access Token
        ///     is valid or not
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsHaveAccessTokenInHeader(HttpRequest request)
        {
            var authenticationHeader = request.Headers[HeaderKey.Authorization].ToString();
            var token = authenticationHeader.Replace(Constants.Constant.AuthenticationTokenType, string.Empty)?.Trim();
            return !string.IsNullOrWhiteSpace(token);
        }

        #endregion

        #region Get Data

        /// <summary>
        ///     Get valid and not expire access token in Authorization Header 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string GetValidAndNotExpireAccessToken(IHeaderDictionary header)
        {
            var authenticationHeader = header[HeaderKey.Authorization].ToString();
            var token = authenticationHeader.Replace(Constants.Constant.AuthenticationTokenType, string.Empty)?.Trim();
            return IsExpireOrInvalidToken(token) ? null : token;
        }

        public static ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            try
            {
                return handler.ValidateToken(token, AuthConfig.TokenValidationParameters, out _);
            }
            catch
            {
                return null;
            }
        }

        public static string GetAccessTokenSubject(string token)
        {
            return GetAccessTokenData<string>(token, JwtRegisteredClaimNames.Sub);
        }

        public static string GetAccessTokenClientId(string token)
        {
            return GetAccessTokenData<string>(token, Constants.Constant.ClientIdKey);
        }

        public static DateTimeOffset? GetAccessTokenExpireOn(string token)
        {
            double? expireOn = GetAccessTokenData<double?>(token, JwtRegisteredClaimNames.Exp);
            return expireOn == null ? (DateTimeOffset?)null : DateTimeHelper.GetDateTimeFromEpoch(expireOn.Value);
        }

        public static T GetAccessTokenData<T>(string token, string key)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(key))
            {
                return default(T);
            }

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