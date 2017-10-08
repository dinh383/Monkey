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
using Monkey.Core.Entities.User;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.Auth;
using Monkey.Data.User;
using Puppy.AutoMapper;
using Puppy.Core.StringUtils;
using Puppy.DataTable;
using Puppy.DataTable.Models.Request;
using Puppy.DataTable.Models.Response;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Business.Logic.Auth
{
    [PerRequestDependency(ServiceType = typeof(IUserBusiness))]
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _userRepository;

        public UserBusiness(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            var isExist = _userRepository.Get(x => x.UserNameNorm == userNameNorm).Any();

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

            var isExist = _userRepository.Get(x => x.EmailNorm == emailNorm && x.EmailConfirmedTime != null).Any();

            if (isExist)
            {
                throw new MonkeyException(ErrorCode.UserEmailNotUnique);
            }
        }

        public void CheckUniquePhone(string phone, int? excludeId = null)
        {
            var isExist = _userRepository.Get(x => x.Phone == phone && x.PhoneConfirmedTime != null).Any();

            if (isExist)
            {
                throw new MonkeyException(ErrorCode.UserEmailNotUnique);
            }
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

        public Task RemoveAsync(int id)
        {
            _userRepository.Delete(new UserEntity
            {
                Id = id
            });

            _userRepository.SaveChanges();

            return Task.CompletedTask;
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

            _userRepository.Update(userEntity, x => x.BannedTime, x => x.BannedRemark);

            _userRepository.SaveChanges();

            return Task.CompletedTask;
        }
    }
}