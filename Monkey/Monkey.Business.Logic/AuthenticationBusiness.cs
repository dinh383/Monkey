#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Logic </Project>
//     <File>
//         <Name> AuthenticationBusiness.cs </Name>
//         <Created> 13/09/17 10:47:35 PM </Created>
//         <Key> 52493530-0bda-4a18-b7bd-470f2f146836 </Key>
//     </File>
//     <Summary>
//         AuthenticationBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.EntityFrameworkCore;
using Monkey.Core.Entities.User;
using Monkey.Core.Exceptions;
using Monkey.Data.User;
using Puppy.Core.StringUtils;
using Puppy.DependencyInjection.Attributes;
using Puppy.Web;
using Puppy.Web.HttpRequestDetection.Device;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Monkey.Business.Logic
{
    [PerRequestDependency(ServiceType = typeof(IAuthenticationBusiness))]
    [PerRequestDependency(ServiceType = typeof(Authentication.Interfaces.IAuthenticationBusiness))]
    public class AuthenticationBusiness : IAuthenticationBusiness
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;

        public AuthenticationBusiness(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task ExpireAllRefreshTokenAsync(int userId)
        {
            var listRefreshToken = await _refreshTokenRepository.Get(x => x.UserId == userId).Select(x =>
                new RefreshTokenEntity
                {
                    Id = x.Id
                }).ToListAsync().ConfigureAwait(true);

            var dateTimeUtcNow = DateTimeOffset.UtcNow.AddSeconds(-1);
            foreach (var refreshTokenEntity in listRefreshToken)
            {
                refreshTokenEntity.ExpireOn = dateTimeUtcNow;
                _refreshTokenRepository.Update(refreshTokenEntity, x => x.RefreshToken, x => x.ExpireOn);
            }

            _refreshTokenRepository.SaveChanges();
        }

        public Task SignInAsync(int id, int clientId, string refreshToken, DateTimeOffset? expireOn)
        {
            var deviceInfo = HttpContext.Current?.Request.GetDeviceInfo();

            var refreshTokenEntity = new RefreshTokenEntity
            {
                RefreshToken = refreshToken,
                ClientId = clientId,
                ExpireOn = expireOn,
                UserId = id,
                TotalUsage = 1,
                DeviceType = deviceInfo?.Type ?? DeviceType.Unknown,
                MarkerName = deviceInfo?.MarkerName,
                MarkerVersion = deviceInfo?.MarkerVersion,
                OsName = deviceInfo?.OsName,
                OsVersion = deviceInfo?.OsVersion,
                EngineName = deviceInfo?.EngineName,
                EngineVersion = deviceInfo?.EngineVersion,
                BrowserName = deviceInfo?.BrowserName,
                BrowserVersion = deviceInfo?.BrowserVersion,
                IpAddress = deviceInfo?.IpAddress,
                CityName = deviceInfo?.CityName,
                CityGeoNameId = deviceInfo?.CityGeoNameId,
                CountryName = deviceInfo?.CountryName,
                CountryGeoNameId = deviceInfo?.CountryGeoNameId,
                CountryIsoCode = deviceInfo?.CountryIsoCode,
                ContinentName = deviceInfo?.ContinentName,
                ContinentGeoNameId = deviceInfo?.ContinentGeoNameId,
                ContinentCode = deviceInfo?.ContinentCode,
                TimeZone = deviceInfo?.TimeZone,
                Latitude = deviceInfo?.Latitude,
                Longitude = deviceInfo?.Longitude,
                AccuracyRadius = deviceInfo?.AccuracyRadius,
                PostalCode = deviceInfo?.PostalCode,
                UserAgent = deviceInfo?.UserAgent,
                DeviceHash = deviceInfo?.DeviceHash
            };

            _refreshTokenRepository.Add(refreshTokenEntity);
            return _refreshTokenRepository.SaveChangesAsync();
        }

        public void CheckValidSignInAsync(string userName, string password, string secretKey)
        {
            userName = StringHelper.Normalize(userName);

            var userInfo = _userRepository.Get(x => x.UserNameNorm == userName)
                .Select(x => new
                {
                    x.PasswordHash,
                    x.PasswordLastUpdatedTime,
                    x.BannedTime,
                    x.BannedRemark
                })
                .Single();

            // Check Password
            if (userInfo.PasswordLastUpdatedTime == null)
            {
                throw new MonkeyException(ErrorCode.UserPasswordIsWrong);
            }

            password = HashPassword(password, userInfo.PasswordLastUpdatedTime.Value, secretKey);

            if (password != userInfo.PasswordHash)
            {
                throw new MonkeyException(ErrorCode.UserPasswordIsWrong);
            }

            // Check Banned
            if (userInfo.BannedTime != null)
            {
                throw new MonkeyException(ErrorCode.UserIsBanned, userInfo.BannedRemark);
            }
        }

        public void CheckValidRefreshToken(string refreshToken, int clientSubject)
        {
            var dateTimeUtcNow = DateTimeOffset.UtcNow;

            var isValidRefreshToken = _refreshTokenRepository.Get().Any(x => x.RefreshToken == refreshToken && x.ClientId == clientSubject && (x.ExpireOn == null || dateTimeUtcNow < x.ExpireOn));

            if (!isValidRefreshToken)
                throw new MonkeyException(ErrorCode.InvalidRefreshToken);
        }

        public string HashPassword(string password, DateTimeOffset hashTime, string secretKey)
        {
            var passwordSalt = hashTime.ToString("O") + secretKey;
            var passwordHash = password.HashPassword(passwordSalt);
            return passwordHash;
        }
    }
}