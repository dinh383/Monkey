#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Logic </Project>
//     <File>
//         <Name> UserBusiness.cs </Name>
//         <Created> 18/07/17 4:51:18 PM </Created>
//         <Key> 9d7c1015-c05c-4dc2-aea4-4b50b1d01bc5 </Key>
//     </File>
//     <Summary>
//         UserBusiness.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.EntityFrameworkCore;
using Monkey.Auth.Helpers;
using Monkey.Business.Auth;
using Monkey.Core.Entities.Auth;
using Monkey.Core.Entities.User;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Monkey.Data.Auth;
using Monkey.Data.User;
using Puppy.AutoMapper;
using Puppy.Core.StringUtils;
using Puppy.DataTable;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enums = Monkey.Core.Constants.Enums;

namespace Monkey.Business.Logic.Auth
{
    [PerRequestDependency(ServiceType = typeof(IUserBusiness))]
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UserBusiness(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public void CheckExistsById(params int[] ids)
        {
            ids = ids.Distinct().ToArray();

            var totalInDb = _userRepository.Get(x => ids.Contains(x.Id)).Count();

            if (totalInDb != ids.Length)
            {
                throw new MonkeyException(ErrorCode.UserNotExist);
            }
        }

        public void CheckExistsBySubject(params string[] subjects)
        {
            subjects = subjects.Distinct().ToArray();

            var totalInDb = _userRepository.Get(x => subjects.Contains(x.GlobalId)).Count();

            if (totalInDb != subjects.Length)
            {
                throw new MonkeyException(ErrorCode.UserNotExist);
            }
        }

        public void CheckUniqueUserName(string userName, int? excludeId = null)
        {
            var userNameNorm = StringHelper.Normalize(userName);

            // Ignore empty case
            if (string.IsNullOrWhiteSpace(userNameNorm))
            {
                return;
            }

            var query = _userRepository.Get(x => x.UserNameNorm == userNameNorm);

            if (excludeId != null)
            {
                query = query.Where(x => x.Id != excludeId);
            }

            var isExist = query.Any();

            if (isExist)
            {
                throw new MonkeyException(ErrorCode.UserNameNotUnique);
            }
        }

        public void CheckExistByUserName(string userName)
        {
            var userNameNorm = StringHelper.Normalize(userName);

            var isExist = _userRepository.Get(x => x.UserNameNorm == userNameNorm).Any();

            if (!isExist)
            {
                throw new MonkeyException(ErrorCode.UserNameNotExist);
            }
        }

        public void CheckUniqueEmail(string email, int? excludeId = null)
        {
            var emailNorm = StringHelper.Normalize(email);

            // Ignore empty case
            if (string.IsNullOrWhiteSpace(emailNorm))
            {
                return;
            }

            var query = _userRepository.Get(x => x.EmailNorm == emailNorm);

            if (excludeId != null)
            {
                query = query.Where(x => x.Id != excludeId);
            }

            var isExist = query.Any();

            if (isExist)
            {
                throw new MonkeyException(ErrorCode.UserEmailNotUnique);
            }
        }

        public void CheckUniquePhone(string phone, int? excludeId = null)
        {
            // Ignore empty case
            if (string.IsNullOrWhiteSpace(phone))
            {
                return;
            }

            var query = _userRepository.Get(x => x.Phone == phone);

            if (excludeId != null)
            {
                query = query.Where(x => x.Id != excludeId);
            }

            var isExist = query.Any();

            if (isExist)
            {
                throw new MonkeyException(ErrorCode.UserPhoneNotUnique);
            }
        }

        public List<int> ListUserIdByPermissions(params Enums.Permission[] permissions)
        {
            List<int> listAdminUserId = _userRepository
                .Get(x => x.Role.Permissions.All(y => permissions.Contains(y.Permission)))
                .Select(x => x.Id)
                .ToList();

            return listAdminUserId;
        }

        public Task<string> CreateUserByEmailAsync(string email, int? roleId)
        {
            var userEntity = new UserEntity
            {
                Email = email,
                EmailNorm = StringHelper.Normalize(email),
                RoleId = roleId
            };

            _userRepository.Add(userEntity);
            _userRepository.SaveChanges();

            return Task.FromResult(userEntity.GlobalId);
        }

        public string GenerateTokenActiveByEmailAsync(string userSubject, string email, out TimeSpan expireIn)
        {
            expireIn = TimeSpan.FromDays(1);

            var expireOn = DateTime.UtcNow.Add(expireIn);

            string token = TokenHelper.GenerateToken(expireOn, nameof(Monkey), new Dictionary<string, string>
            {
                {nameof(userSubject), userSubject},
                {nameof(email), email}
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

        public string GenerateTokenActiveByPhoneAsync(string userSubject, string phone, out TimeSpan expireIn)
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

        public async Task RemoveAsync(int id)
        {
            // Soft delete user
            _userRepository.Delete(new UserEntity
            {
                Id = id
            });

            _refreshTokenRepository.SaveChanges();

            // Expire all their refresh token
            var listRefreshToken = await _refreshTokenRepository
                .Get(x => x.UserId == id)
                .Select(x =>
                    new RefreshTokenEntity
                    {
                        Id = x.Id
                    })
                .ToListAsync().ConfigureAwait(true);

            var dateTimeUtcNow = DateTimeOffset.UtcNow.AddSeconds(-1);

            foreach (var refreshTokenEntity in listRefreshToken)
            {
                refreshTokenEntity.ExpireOn = dateTimeUtcNow;
                _refreshTokenRepository.Update(refreshTokenEntity, x => x.RefreshToken, x => x.ExpireOn);
            }

            _userRepository.SaveChanges();
        }

        public Task<UserModel> GetAsync(int id)
        {
            var userEntity = _userRepository.Get(x => x.Id == id).Single();
            var userModel = userEntity.MapTo<UserModel>();
            return Task.FromResult(userModel);
        }

        public Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model)
        {
            var listData = _userRepository.Get().QueryTo<UserModel>();

            var result = listData.GetDataTableResponse(model);

            return Task.FromResult(result);
        }

        public Task UpdateAsync(UserUpdateModel model)
        {
            var userEntity = model.MapTo<UserEntity>();

            userEntity.BannedTime = model.IsBanned ? DateTimeOffset.UtcNow : (DateTimeOffset?)null;

            userEntity.BannedRemark = model.IsBanned ? null : userEntity.BannedRemark;

            _userRepository.Update(userEntity, x => x.RoleId, x => x.BannedTime, x => x.BannedRemark);

            _userRepository.SaveChanges();

            return Task.CompletedTask;
        }
    }
}