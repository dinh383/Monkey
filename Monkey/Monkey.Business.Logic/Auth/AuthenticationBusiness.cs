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

            var user = _userRepository.Get(x => x.UserNameNorm == userName).Single();

            // Get logged in user
            var loggedInUserModel = user.MapTo<LoggedInUserModel>();

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

        #region Get Logged In User

        public Task<LoggedInUserModel> GetLoggedInUserBySubjectAsync(string subject, CancellationToken cancellationToken = default(CancellationToken))
        {
            var loggedInUser = _userRepository.Get(x => x.GlobalId == subject).QueryTo<LoggedInUserModel>().Single();

            return Task.FromResult(loggedInUser);
        }

        public Task<LoggedInUserModel> GetLoggedInUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default(CancellationToken))
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

        #region Refresh Token

        public void CheckValidRefreshToken(string refreshToken, int? clientId)
        {
            var dateTimeUtcNow = DateTimeOffset.UtcNow;

            var isValidRefreshToken = _refreshTokenRepository.Get().Any(x => x.RefreshToken == refreshToken && x.ClientId == clientId && (x.ExpireOn == null || dateTimeUtcNow < x.ExpireOn));

            if (!isValidRefreshToken)
                throw new MonkeyException(ErrorCode.InvalidRefreshToken);
        }

        public Task ExpireAllRefreshTokenAsync(string subject, CancellationToken cancellationToken = default(CancellationToken))
        {
            var listRefreshToken =
                _refreshTokenRepository.Get(x => x.User.GlobalId == subject)
                    .Select(x =>
                        new RefreshTokenEntity
                        {
                            Id = x.Id
                        }).ToList();

            var dateTimeUtcNow = DateTimeOffset.UtcNow.AddSeconds(-1);

            foreach (var refreshTokenEntity in listRefreshToken)
            {
                refreshTokenEntity.ExpireOn = dateTimeUtcNow;
                _refreshTokenRepository.Update(refreshTokenEntity, x => x.RefreshToken, x => x.ExpireOn);
            }

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            return Task.CompletedTask;
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

        public Task ConfirmEmailAsync(string subject, string newUserName, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
        {
            var utcNow = DateTimeOffset.UtcNow;

            var userEntity = _userRepository.Get(x => x.GlobalId == subject).Single();

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

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            return Task.CompletedTask;
        }

        public void ExpireTokenConfirmEmail(string token)
        {
            var userId = _userRepository.Get(x => x.ConfirmEmailToken == token).Select(x => x.Id).FirstOrDefault();

            if (userId == default(int))
            {
                return;
            }

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
            var isValid = _userRepository.Get(x => x.ConfirmEmailToken == token && x.ConfirmEmailTokenExpireOn >= checkTime).Any();
            return !isValid;
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

        public Task ConfirmPhoneAsync(string subject, string newUserName, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
        {
            var utcNow = DateTimeOffset.UtcNow;

            var userEntity = _userRepository.Get(x => x.GlobalId == subject).Single();

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

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            return Task.CompletedTask;
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

        public Task SetPasswordAsync(string subject, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            var utcNow = DateTimeOffset.UtcNow;
            var userEntity = _userRepository.Get(x => x.GlobalId == subject).Single();

            userEntity.PasswordHash = PasswordHelper.HashPassword(password, utcNow);
            userEntity.PasswordLastUpdatedTime = utcNow;
            _userRepository.Update(userEntity, x => x.PasswordHash, x => x.PasswordLastUpdatedTime);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            return Task.CompletedTask;
        }

        public void ExpireTokenSetPassword(string token)
        {
            var userId = _userRepository.Get(x => x.SetPasswordToken == token).Select(x => x.Id).FirstOrDefault();

            if (userId == default(int))
            {
                return;
            }

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
            var isValid = _userRepository.Get(x => x.SetPasswordToken == token && x.SetPasswordTokenExpireOn >= checkTime).Any();
            return !isValid;
        }

        #endregion
    }
}