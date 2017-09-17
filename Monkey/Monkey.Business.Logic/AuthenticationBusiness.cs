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
using Monkey.Core.Entities.User;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.User;
using Monkey.Data.User;
using Puppy.AutoMapper;
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

        #region Sign In

        public void CheckExistsByUserName(params string[] userNames)
        {
            userNames = userNames.Distinct().Select(StringHelper.Normalize).ToArray();

            var totalInDb = _userRepository.Get(x => userNames.Contains(x.UserNameNorm)).Count();

            if (totalInDb != userNames.Length)
            {
                throw new MonkeyException(ErrorCode.UserNameNotExist);
            }
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
                throw new MonkeyException(ErrorCode.UserPasswordIsWrong);
            }
            password = PasswordHelper.HashPassword(password, user.PasswordLastUpdatedTime.Value);
            if (password != user.PasswordHash)
            {
                throw new MonkeyException(ErrorCode.UserPasswordIsWrong);
            }

            // Check Banned
            if (user.BannedTime != null)
            {
                throw new MonkeyException(ErrorCode.UserIsBanned, user.BannedRemark);
            }
        }

        public LoggedInUserModel SignIn(int clientId, string userName, string password, out string refreshToken)
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
        private string GenerateRefreshToken(int userId, int clientId)
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

        #endregion

        #region Get By Subject

        public void CheckExistsBySubject(params string[] subjects)
        {
            subjects = subjects.Distinct().ToArray();

            var totalInDb = _userRepository.Get(x => subjects.Contains(x.GlobalId)).Count();

            if (totalInDb != subjects.Length)
            {
                throw new MonkeyException(ErrorCode.UserNotExist);
            }
        }

        public async Task<LoggedInUserModel> GetBySubjectAsync(string subject)
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

        #endregion

        #region Get By Refresh Token and Expire All Refresh Token

        public void CheckValidRefreshToken(int clientId, string refreshToken)
        {
            var dateTimeUtcNow = DateTimeOffset.UtcNow;

            var isValidRefreshToken = _refreshTokenRepository.Get().Any(x => x.RefreshToken == refreshToken && x.ClientId == clientId && (x.ExpireOn == null || dateTimeUtcNow < x.ExpireOn));

            if (!isValidRefreshToken)
                throw new MonkeyException(ErrorCode.InvalidRefreshToken);
        }

        public async Task<LoggedInUserModel> GetByRefreshTokenAsync(string refreshToken)
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

        #endregion

        #region Create and Active

        public Task<string> CreateUserAsync(string email)
        {
            var userEntity = new UserEntity
            {
                Email = email,
                EmailNorm = StringHelper.Normalize(email)
            };

            _userRepository.Add(userEntity);
            _userRepository.SaveChanges();
            return Task.FromResult(userEntity.GlobalId);
        }

        public async Task ActiveByEmailAsync(string subject, string newUserName, string newPassword)
        {
            var utcNow = DateTimeOffset.UtcNow;

            var userEntity = await _userRepository.Get(x => x.GlobalId == subject).SingleAsync().ConfigureAwait(true);

            userEntity.EmailConfirmedTime = utcNow;
            userEntity.ActiveTime = utcNow;
            userEntity.UserName = newUserName;
            userEntity.UserNameNorm = StringHelper.Normalize(newUserName);
            userEntity.PasswordHash = PasswordHelper.HashPassword(newPassword, utcNow);
            userEntity.PasswordLastUpdatedTime = utcNow;

            _userRepository.Update(userEntity,
                x => x.UserName,
                x => x.UserNameNorm,
                x => x.ActiveTime,
                x => x.PhoneConfirmedTime,
                x => x.PasswordHash,
                x => x.PasswordLastUpdatedTime);

            _userRepository.SaveChanges();
        }

        public async Task ActiveByPhoneAsync(string subject, string newUserName, string newPassword)
        {
            var utcNow = DateTimeOffset.UtcNow;

            var userEntity = await _userRepository.Get(x => x.GlobalId == subject).SingleAsync().ConfigureAwait(true);

            userEntity.PhoneConfirmedTime = utcNow;
            userEntity.ActiveTime = utcNow;
            userEntity.UserName = newUserName;
            userEntity.UserNameNorm = StringHelper.Normalize(newUserName);
            userEntity.PasswordHash = PasswordHelper.HashPassword(newPassword, utcNow);
            userEntity.PasswordLastUpdatedTime = utcNow;

            _userRepository.Update(userEntity,
                x => x.UserName,
                x => x.UserNameNorm,
                x => x.ActiveTime,
                x => x.PhoneConfirmedTime,
                x => x.PasswordHash,
                x => x.PasswordLastUpdatedTime);

            _userRepository.SaveChanges();
        }

        #endregion
    }
}