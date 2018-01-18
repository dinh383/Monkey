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
using System.Threading;
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

        #region Get

        public Task<LoggedInUserModel> GetLoggedInUserBySubjectAsync(string subject, CancellationToken cancellationToken = default)
        {
            var loggedInUser = _userRepository.Get(x => x.GlobalId == subject).QueryTo<LoggedInUserModel>().Single();

            return Task.FromResult(loggedInUser);
        }

        public Task<LoggedInUserModel> GetLoggedInUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            var refreshTokenEntity = _refreshTokenRepository.Get(x => x.RefreshToken == refreshToken).Select(x => new RefreshTokenEntity
            {
                Id = x.Id,
                TotalUsage = x.TotalUsage,
                UserId = x.UserId
            }).Single();

            var loggedInUser = _userRepository.Get(x => x.Id == refreshTokenEntity.UserId).QueryTo<LoggedInUserModel>().Single();

            // Increase total usage
            refreshTokenEntity.TotalUsage++;

            _refreshTokenRepository.Update(refreshTokenEntity, x => x.TotalUsage);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _refreshTokenRepository.SaveChanges();

            return Task.FromResult(loggedInUser);
        }

        #endregion

        #region Sign In

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

            var user = _userRepository.Get(x => x.UserNameNorm == userName).Single();

            // GetElastic logged in user
            var loggedInUserModel = user.MapTo<LoggedInUserModel>();
            loggedInUserModel.ClientId = clientId;

            // Generate and save refresh token
            refreshToken = GenerateRefreshToken(loggedInUserModel.Id, loggedInUserModel.ClientId);

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

        #region Refresh Token

        public Task ExpireAllRefreshTokenAsync(string subject, CancellationToken cancellationToken = default)
        {
            var listRefreshToken =
                _refreshTokenRepository.Get(x => x.User.GlobalId == subject)
                    .Select(x =>
                        new RefreshTokenEntity
                        {
                            Id = x.Id
                        }).ToList();

            var systemTimePast = SystemUtils.SystemTimeNow.AddSeconds(-1);

            foreach (var refreshTokenEntity in listRefreshToken)
            {
                refreshTokenEntity.ExpireOn = systemTimePast;
                _refreshTokenRepository.Update(refreshTokenEntity, x => x.RefreshToken, x => x.ExpireOn);
            }

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            return Task.CompletedTask;
        }

        #endregion

        #region Get Password

        public string GenerateTokenSetPassword(string userSubject, string email, out TimeSpan expireIn)
        {
            expireIn = TimeSpan.FromDays(1);

            var expireOn = SystemUtils.SystemTimeNow.Add(expireIn).DateTime;

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

        public string GenerateTokenConfirmEmail(string userSubject, string email, out TimeSpan expireIn)
        {
            expireIn = TimeSpan.FromDays(1);

            var expireOn = SystemUtils.SystemTimeNow.Add(expireIn).DateTime;

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

        public string GenerateTokenConfirmPhone(string userSubject, string phone, out TimeSpan expireIn)
        {
            expireIn = TimeSpan.FromMinutes(10);

            var expireOn = SystemUtils.SystemTimeNow.Add(expireIn).DateTime;

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

        public void ExpireTokenSetPassword(string token)
        {
            var userId = _userRepository.Get(x => x.SetPasswordToken == token).Select(x => x.Id).FirstOrDefault();

            if (userId == default)
            {
                return;
            }

            _userRepository.Update(new UserEntity
            {
                Id = userId,
                SetPasswordTokenExpireOn = SystemUtils.SystemTimeNow
            }, x => x.SetPasswordTokenExpireOn);

            _userRepository.SaveChanges();
        }

        public void ExpireTokenConfirmEmail(string token)
        {
            var userId = _userRepository.Get(x => x.ConfirmEmailToken == token).Select(x => x.Id).FirstOrDefault();

            if (userId == default)
            {
                return;
            }

            _userRepository.Update(new UserEntity
            {
                Id = userId,
                ConfirmEmailTokenExpireOn = SystemUtils.SystemTimeNow
            }, x => x.ConfirmEmailTokenExpireOn);

            _userRepository.SaveChanges();
        }

        public void ExpireTokenConfirmPhone(string token)
        {
            var userId = _userRepository.Get(x => x.ConfirmPhoneToken == token).Select(x => x.Id).Single();

            _userRepository.Update(new UserEntity
            {
                Id = userId,
                ConfirmPhoneTokenExpireOn = SystemUtils.SystemTimeNow
            }, x => x.ConfirmPhoneTokenExpireOn);

            _userRepository.SaveChanges();
        }

        #endregion

        #region Set Password

        public Task SetPasswordAsync(string subject, string password, CancellationToken cancellationToken = default)
        {
            var systemTimeNow = SystemUtils.SystemTimeNow;

            var userEntity = _userRepository.Get(x => x.GlobalId == subject).Single();

            userEntity.PasswordHash = PasswordHelper.HashPassword(password, systemTimeNow);
            userEntity.PasswordLastUpdatedTime = systemTimeNow;
            _userRepository.Update(userEntity, x => x.PasswordHash, x => x.PasswordLastUpdatedTime);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            return Task.CompletedTask;
        }

        public Task ConfirmEmailAsync(string subject, string newUserName, string newPassword, CancellationToken cancellationToken = default)
        {
            var systemTimeNow = SystemUtils.SystemTimeNow;

            var userEntity = _userRepository.Get(x => x.GlobalId == subject).Single();

            userEntity.EmailConfirmedTime = systemTimeNow;
            userEntity.ActiveTime = systemTimeNow;

            userEntity.UserName = newUserName;
            userEntity.UserNameNorm = StringHelper.Normalize(newUserName);

            userEntity.PasswordHash = PasswordHelper.HashPassword(newPassword, systemTimeNow);
            userEntity.PasswordLastUpdatedTime = systemTimeNow;

            _userRepository.Update(userEntity,
                x => x.UserName,
                x => x.UserNameNorm,
                x => x.EmailConfirmedTime,
                x => x.ActiveTime,
                x => x.PasswordHash,
                x => x.PasswordLastUpdatedTime);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            return Task.CompletedTask;
        }

        public Task ConfirmPhoneAsync(string subject, string newUserName, string newPassword, CancellationToken cancellationToken = default)
        {
            var systemTimeNow = SystemUtils.SystemTimeNow;

            var userEntity = _userRepository.Get(x => x.GlobalId == subject).Single();

            userEntity.PhoneConfirmedTime = systemTimeNow;
            userEntity.ActiveTime = systemTimeNow;

            userEntity.UserName = newUserName;
            userEntity.UserNameNorm = StringHelper.Normalize(newUserName);

            userEntity.PasswordHash = PasswordHelper.HashPassword(newPassword, systemTimeNow);
            userEntity.PasswordLastUpdatedTime = systemTimeNow;

            _userRepository.Update(userEntity,
                x => x.UserName,
                x => x.UserNameNorm,
                x => x.PhoneConfirmedTime,
                x => x.ActiveTime,
                x => x.PasswordHash,
                x => x.PasswordLastUpdatedTime);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            return Task.CompletedTask;
        }

        #endregion

        #region Validation

        public bool IsExpireOrInvalidSetPasswordToken(string token)
        {
            var checkTime = SystemUtils.SystemTimeNow;
            var isValid = _userRepository.Get(x => x.SetPasswordToken == token && x.SetPasswordTokenExpireOn >= checkTime).Any();
            return !isValid;
        }

        public bool IsExpireOrInvalidConfirmEmailToken(string token)
        {
            var checkTime = SystemUtils.SystemTimeNow;
            var isValid = _userRepository.Get(x => x.ConfirmEmailToken == token && x.ConfirmEmailTokenExpireOn >= checkTime).Any();
            return !isValid;
        }

        public void CheckValidRefreshToken(string refreshToken, int? clientId)
        {
            var systemTimeNow = SystemUtils.SystemTimeNow;

            var isValidRefreshToken = _refreshTokenRepository.Get().Any(x => x.RefreshToken == refreshToken && x.ClientId == clientId && (x.ExpireOn == null || systemTimeNow < x.ExpireOn));

            if (!isValidRefreshToken)
                throw new MonkeyException(ErrorCode.InvalidRefreshToken);
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

        #endregion
    }
}