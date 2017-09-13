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
using Monkey.Authentication;
using Monkey.Core.Exceptions;
using Monkey.Data.Entities.User;
using Monkey.Data.Interfaces;
using Monkey.Model.Models.User;
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
    public class AuthenticationBusiness : IAuthenticationBusiness
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;

        public AuthenticationBusiness(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LoggedUserModel> SignInAsync(string username, string password)
        {
            username = StringHelper.Normalize(username);

            var user = await _userRepository.Get(x => x.UserNameNorm == username).Include(x => x.Role)
                .ThenInclude(x => x.Permissions).SingleAsync().ConfigureAwait(true);

            var listPermission = user.Role.Permissions.Select(c => c.Permission).ToList();

            CheckPasswordHash(password, user.PasswordSalt, user.PasswordHash);

            var loggedUser = new LoggedUserModel
            {
                Id = user.Id,
                Username = user.UserName,
                ListPermission = listPermission
            };

            return loggedUser;
        }

        public async Task<LoggedUserModel> GetUserInfoByGlobalIdAsync(string globalId)
        {
            var user = await _userRepository.Get(x => x.GlobalId == globalId).Include(x => x.Role)
                .ThenInclude(x => x.Permissions).SingleAsync().ConfigureAwait(true);

            var listPermission = user.Role.Permissions.Select(c => c.Permission).ToList();

            var loggedUser = new LoggedUserModel
            {
                Id = user.Id,
                Username = user.UserName,
                ListPermission = listPermission
            };
            return loggedUser;
        }

        public async Task<LoggedUserModel> GetUserInfoAsync(string refreshToken)
        {
            var refreshTokenEntity = _refreshTokenRepository.Get(x => x.RefreshToken == refreshToken).Select(x => new RefreshTokenEntity
            {
                Id = x.Id,
                TotalUsage = x.TotalUsage,
                User = x.User
            }).Single();

            var listPermission = await _userRepository.Get(x => x.Id == refreshTokenEntity.UserId)
                .SelectMany(x => x.Role.Permissions.Select(y => y.Permission)).ToListAsync().ConfigureAwait(true);

            var loggedUser = new LoggedUserModel
            {
                Id = refreshTokenEntity.User.Id,
                Username = refreshTokenEntity.User.UserName,
                ListPermission = listPermission
            };

            // Increase total usage
            refreshTokenEntity.TotalUsage++;

            _refreshTokenRepository.Update(refreshTokenEntity, x => x.TotalUsage);

            _refreshTokenRepository.SaveChanges();

            return loggedUser;
        }

        public Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTimeOffset? expireOn)
        {
            var deviceInfo = HttpContext.Current?.Request.GetDeviceInfo();

            var refreshTokenEntity = new RefreshTokenEntity
            {
                RefreshToken = refreshToken,
                ExpireOn = expireOn,
                UserId = userId,
                TotalUsage = 1,
                Type = deviceInfo?.Type ?? DeviceType.Unknown,
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

        public void CheckValidRefreshToken(string refreshToken)
        {
            var dateTimeUtcNow = DateTimeOffset.UtcNow;

            var isValidRefreshToken = _refreshTokenRepository.Get().Any(x =>
                x.RefreshToken == refreshToken && (x.ExpireOn == null || dateTimeUtcNow < x.ExpireOn));
            if (!isValidRefreshToken)
                throw new MonkeyException(ErrorCode.InvalidRefreshToken);
        }

        private static void CheckPasswordHash(string password, string passwordSalt, string passwordHash)
        {
            password = HashPassword(password, passwordSalt);
            if (password == passwordHash) return;
            throw new MonkeyException(ErrorCode.UserNotExist);
        }

        private static string HashPassword(string password, string salt)
        {
            var passwordSalt = password + salt;
            var passwordHash = passwordSalt.GetHmacSha512(AuthenticationConfig.SecretKey);
            return passwordHash;
        }
    }
}