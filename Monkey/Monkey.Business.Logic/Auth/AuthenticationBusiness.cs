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
using Monkey.Auth.Helpers;
using Monkey.Business.Auth;
using Monkey.Core.Entities.Auth;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Monkey.Data.Auth;
using Monkey.Data.User;
using Puppy.AutoMapper;
using Puppy.Core.StringUtils;
using Puppy.DependencyInjection.Attributes;
using Puppy.Web.HttpUtils;
using Puppy.Web.HttpUtils.HttpDetection.Device;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Monkey.Business.Logic.Auth
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

        public void CheckValidSignIn(string userName, string password)
        {
            userName = StringHelper.Normalize(userName);

            var user = _userRepository.Get(x => x.UserNameNorm == userName).Select(x => new
            {
                x.PasswordHash,
                x.PasswordLastUpdatedTime,
                x.BannedTime,
                x.BannedRemark
            }).Single();

            // Check Password
            if (user.PasswordLastUpdatedTime == null)
            {
                throw new MonkeyException(ErrorCode.UserPasswordWrong);
            }
            password = PasswordHelper.HashPassword(password, user.PasswordLastUpdatedTime.Value);
            if (password != user.PasswordHash)
            {
                throw new MonkeyException(ErrorCode.UserPasswordWrong);
            }

            // Check Banned
            if (user.BannedTime != null)
            {
                throw new MonkeyException(ErrorCode.UserBanned, user.BannedRemark);
            }
        }

        public LoggedInUserModel SignIn(string userName, string password, out string refreshToken, int? clientId)
        {
            userName = StringHelper.Normalize(userName);

            var user = _userRepository.Get(x => x.UserNameNorm == userName)
                .Include(x => x.Profile)
                .Include(x => x.Role).ThenInclude(x => x.Permissions)
                .Single();

            // Generate and save refresh token
            refreshToken = GenerateRefreshToken(user.Id, clientId);

            // Get logged in user
            var loggedInUserModel = user.MapTo<LoggedInUserModel>();
            var listPermission = user.Role?.Permissions?.Select(c => c.Permission).ToList();

            loggedInUserModel.ListPermission = listPermission;
            user.Profile.MapTo(loggedInUserModel);

            return loggedInUserModel;
        }

        /// <summary>
        ///     Generate and save refresh token never expire 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private string GenerateRefreshToken(int userId, int? clientId)
        {
            var refreshToken = Guid.NewGuid().ToString();

            var deviceInfo = HttpContext.Current?.Request.GetDeviceInfo();

            var refreshTokenEntity = new RefreshTokenEntity
            {
                RefreshToken = refreshToken,
                ClientId = clientId,
                ExpireOn = null,
                UserId = userId,
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
            _refreshTokenRepository.SaveChanges();

            return refreshToken;
        }

        public async Task<LoggedInUserModel> GetLoggedInUserBySubjectAsync(string subject)
        {
            var user = await _userRepository.Get(x => x.GlobalId == subject)
                .Include(x => x.Profile)
                .Include(x => x.Role).ThenInclude(x => x.Permissions)
                .SingleAsync()
                .ConfigureAwait(true);

            var listPermission = user.Role?.Permissions?.Select(c => c.Permission).ToList();

            var loggedUser = user.MapTo<LoggedInUserModel>();
            loggedUser.ListPermission = listPermission;
            user.Profile.MapTo(loggedUser);

            return loggedUser;
        }

        public async Task<LoggedInUserModel> GetLoggedInUserByRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenEntity = await _refreshTokenRepository.Get(x => x.RefreshToken == refreshToken)
                .Include(x => x.User)
                .ThenInclude(x => x.Profile)
                .SingleAsync().ConfigureAwait(true);

            var listPermission = await _userRepository.Get(x => x.Id == refreshTokenEntity.UserId)
                .SelectMany(x => x.Role.Permissions.Select(y => y.Permission)).ToListAsync().ConfigureAwait(true);

            var loggedUser = refreshTokenEntity.User.MapTo<LoggedInUserModel>();
            loggedUser.ListPermission = listPermission;
            refreshTokenEntity.User.Profile.MapTo(loggedUser);

            // Increase total usage
            refreshTokenEntity.TotalUsage++;

            _refreshTokenRepository.Update(refreshTokenEntity, x => x.TotalUsage);

            _refreshTokenRepository.SaveChanges();

            return loggedUser;
        }

        public void CheckValidRefreshToken(string refreshToken, int? clientId)
        {
            var dateTimeUtcNow = DateTimeOffset.UtcNow;

            var isValidRefreshToken = _refreshTokenRepository.Get().Any(x => x.RefreshToken == refreshToken && x.ClientId == clientId && (x.ExpireOn == null || dateTimeUtcNow < x.ExpireOn));

            if (!isValidRefreshToken)
                throw new MonkeyException(ErrorCode.InvalidRefreshToken);
        }

        public async Task ExpireAllRefreshTokenAsync(string subject)
        {
            var listRefreshToken = await _refreshTokenRepository.Get(x => x.User.GlobalId == subject).Select(x =>
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
    }
}