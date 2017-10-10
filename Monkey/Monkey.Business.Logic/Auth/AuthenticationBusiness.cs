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
using Monkey.Core;
using Monkey.Core.Entities.Auth;
using Monkey.Core.Entities.User;
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
using System.Collections.Generic;
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

        #region SignIn

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

        #endregion

        #region Get Logged In User

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

        #endregion

        #region Refresh Token

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

        #endregion

        #region Confirm Email

        public string GenerateTokenConfirmEmail(string userSubject, string email, out TimeSpan expireIn)
        {
            expireIn = TimeSpan.FromDays(1);

            var expireOn = DateTime.UtcNow.Add(expireIn);

            EmailTokenModel emailTokenModel = new EmailTokenModel
            {
                Email = email,
                Subject = userSubject
            };

            string token = TokenHelper.GenerateToken(expireOn, nameof(Monkey), new Dictionary<string, string>
            {
                {nameof(emailTokenModel.Subject), emailTokenModel.Subject},
                {nameof(emailTokenModel.Email), emailTokenModel.Email}
            });

            var userId = _userRepository.Get(x => x.GlobalId == userSubject && x.Email == email).Select(x => x.Id).Single();

            _userRepository.Update(new UserEntity
            {
                Id = userId,
                ConfirmEmailToken = token,
                ConfirmEmailTokenExpireOn = expireOn
            }, x => x.ConfirmEmailToken, x => x.ConfirmEmailTokenExpireOn);

            _userRepository.SaveChanges();

            return token;
        }

        public async Task ConfirmEmailAsync(string subject, string newUserName, string newPassword)
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

        public void ExpireTokenConfirmEmail(string token)
        {
            var userId = _userRepository.Get(x => x.ConfirmPhoneToken == token).Select(x => x.Id).Single();

            _userRepository.Update(new UserEntity
            {
                Id = userId,
                ConfirmEmailTokenExpireOn = DateTimeOffset.UtcNow
            }, x => x.ConfirmEmailTokenExpireOn);

            _userRepository.SaveChanges();
        }

        public bool IsExpireOrInvalidConfirmEmailToken(string token)
        {
            var checkTime = DateTimeOffset.UtcNow;
            return _userRepository.Get(x => x.ConfirmEmailToken == token && x.ConfirmEmailTokenExpireOn >= checkTime).Any();
        }

        #endregion

        #region Confirm Phone

        public string GenerateTokenConfirmPhone(string userSubject, string phone, out TimeSpan expireIn)
        {
            expireIn = TimeSpan.FromMinutes(10);

            var expireOn = DateTimeOffset.UtcNow.Add(expireIn);

            var userId = _userRepository.Get(x => x.GlobalId == userSubject && x.Phone == phone).Select(x => x.Id).Single();

            string token = StringHelper.GetRandomString(4, StringHelper.NumberChars);

            _userRepository.Update(new UserEntity
            {
                Id = userId,
                ConfirmPhoneToken = token,
                ConfirmPhoneTokenExpireOn = expireOn
            }, x => x.ConfirmPhoneToken, x => x.ConfirmPhoneTokenExpireOn);

            _userRepository.SaveChanges();

            return token;
        }

        public async Task ConfirmPhoneAsync(string subject, string newUserName, string newPassword)
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

        public void ExpireTokenConfirmPhone(string token)
        {
            var userId = _userRepository.Get(x => x.ConfirmPhoneToken == token).Select(x => x.Id).Single();

            _userRepository.Update(new UserEntity
            {
                Id = userId,
                ConfirmPhoneTokenExpireOn = DateTimeOffset.UtcNow
            }, x => x.ConfirmPhoneTokenExpireOn);

            _userRepository.SaveChanges();
        }

        #endregion

        #region Set Password

        public string GenerateTokenSetPassword(string userSubject, string email, out TimeSpan expireIn)
        {
            expireIn = TimeSpan.FromDays(1);

            var expireOn = DateTime.UtcNow.Add(expireIn);

            EmailTokenModel emailTokenModel = new EmailTokenModel
            {
                Email = email,
                Subject = userSubject
            };

            string token = TokenHelper.GenerateToken(expireOn, nameof(Monkey), new Dictionary<string, string>
            {
                {nameof(emailTokenModel.Subject), emailTokenModel.Subject},
                {nameof(emailTokenModel.Email), emailTokenModel.Email}
            });

            var userId = _userRepository.Get(x => x.GlobalId == userSubject && x.Email == email).Select(x => x.Id).Single();

            _userRepository.Update(new UserEntity
            {
                Id = userId,
                SetPasswordToken = token,
                SetPasswordTokenExpireOn = expireOn
            }, x => x.SetPasswordToken, x => x.SetPasswordTokenExpireOn);

            _userRepository.SaveChanges();

            return token;
        }

        public async Task SetPasswordAsync(string subject, string password)
        {
            var utcNow = DateTimeOffset.UtcNow;
            var userEntity = await _userRepository.Get(x => x.GlobalId == subject).SingleAsync().ConfigureAwait(true);
            userEntity.PasswordHash = PasswordHelper.HashPassword(password, utcNow);
            userEntity.PasswordLastUpdatedTime = utcNow;
            _userRepository.Update(userEntity, x => x.PasswordHash, x => x.PasswordLastUpdatedTime);
            _userRepository.SaveChanges();
        }

        public void ExpireTokenSetPassword(string token)
        {
            var userId = _userRepository.Get(x => x.SetPasswordToken == token).Select(x => x.Id).Single();

            _userRepository.Update(new UserEntity
            {
                Id = userId,
                SetPasswordTokenExpireOn = DateTimeOffset.UtcNow
            }, x => x.SetPasswordTokenExpireOn);

            _userRepository.SaveChanges();
        }

        public void CheckCurrentPassword(string currentPassword)
        {
            var user = _userRepository.Get(x => x.Id == LoggedInUser.Current.Id).Select(x => new
            {
                x.PasswordHash,
                x.PasswordLastUpdatedTime,
            }).Single();

            // Check Password
            if (user.PasswordLastUpdatedTime == null)
            {
                throw new MonkeyException(ErrorCode.UserPasswordWrong);
            }

            var password = PasswordHelper.HashPassword(currentPassword, user.PasswordLastUpdatedTime.Value);

            if (password != user.PasswordHash)
            {
                throw new MonkeyException(ErrorCode.UserPasswordWrong);
            }
        }

        public bool IsExpireOrInvalidSetPasswordToken(string token)
        {
            var checkTime = DateTimeOffset.UtcNow;
            return _userRepository.Get(x => x.SetPasswordToken == token && x.SetPasswordTokenExpireOn >= checkTime).Any();
        }

        #endregion
    }
}