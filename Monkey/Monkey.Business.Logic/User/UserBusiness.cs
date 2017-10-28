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

using Monkey.Business.User;
using Monkey.Core;
using Monkey.Core.Entities.Auth;
using Monkey.Core.Entities.User;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.User;
using Monkey.Data;
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
using System.Threading;
using System.Threading.Tasks;
using Enums = Monkey.Core.Constants.Enums;

namespace Monkey.Business.Logic.User
{
    [PerRequestDependency(ServiceType = typeof(IUserBusiness))]
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly IImageRepository _imageRepository;

        public UserBusiness(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IProfileRepository profileRepository,
            IImageRepository imageRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _profileRepository = profileRepository;
            _imageRepository = imageRepository;
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

            var query = _userRepository.Get(x => x.UserNameNorm == userNameNorm && x.ActiveTime != null);

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

            var query = _userRepository.Get(x => x.EmailNorm == emailNorm && x.ActiveTime != null);

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

            var query = _userRepository.Get(x => x.Phone == phone && x.ActiveTime != null);

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

        public void CheckExistByPhone(string phone)
        {
            var isExist = _userRepository.Get(x => x.Phone == phone).Any();

            if (!isExist)
            {
                throw new MonkeyException(ErrorCode.UserPhoneNotExist);
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

        public Task<CreateUserResultModel> CreateUserByEmailAsync(string email, int? roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userEntity = new UserEntity
            {
                Email = email,
                EmailNorm = StringHelper.Normalize(email),
                UserName = email,
                RoleId = roleId,
                Profile = new ProfileEntity()
            };

            _userRepository.Add(userEntity);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            var result = userEntity.MapTo<CreateUserResultModel>();

            return Task.FromResult(result);
        }

        public Task<CreateUserResultModel> CreateUserByPhoneAsync(string phone, int? roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userEntity = new UserEntity
            {
                Phone = phone,
                UserName = phone,
                RoleId = roleId,
                Profile = new ProfileEntity()
            };

            _userRepository.Add(userEntity);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            var result = userEntity.MapTo<CreateUserResultModel>();

            return Task.FromResult(result);
        }

        public Task RemoveAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Soft delete user
            _userRepository.Delete(new UserEntity
            {
                Id = id
            });

            _refreshTokenRepository.SaveChanges();

            // Expire all their refresh token
            var listRefreshToken = _refreshTokenRepository
                .Get(x => x.UserId == id)
                .Select(x =>
                    new RefreshTokenEntity
                    {
                        Id = x.Id
                    })
                .ToList();

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

        public Task<UserModel> GetAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userModel = _userRepository.Get(x => x.Id == id).QueryTo<UserModel>().FirstOrDefault();
            return Task.FromResult(userModel);
        }

        public Task<UserModel> GetByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            string emailNorm = StringHelper.Normalize(email);
            var userModel = _userRepository.Get(x => x.EmailNorm == emailNorm).QueryTo<UserModel>().FirstOrDefault();
            return Task.FromResult(userModel);
        }

        public Task<UserModel> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userModel = _userRepository.Get(x => x.Phone == phone).QueryTo<UserModel>().FirstOrDefault();
            return Task.FromResult(userModel);
        }

        public Task<UserModel> GetBySubjectAsync(string subject, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userModel = _userRepository.Get(x => x.GlobalId == subject).QueryTo<UserModel>().FirstOrDefault();
            return Task.FromResult(userModel);
        }

        public Task<DataTableResponseDataModel> GetDataTableAsync(DataTableParamModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            var listData = _userRepository.Get().QueryTo<UserModel>();

            var result = listData.GetDataTableResponse(model);

            return Task.FromResult(result);
        }

        public Task UpdateAsync(UserUpdateModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userEntity = model.MapTo<UserEntity>();

            userEntity.BannedTime = model.IsBanned ? DateTimeOffset.UtcNow : (DateTimeOffset?)null;

            userEntity.BannedRemark = model.IsBanned ? null : userEntity.BannedRemark;

            _userRepository.Update(userEntity, x => x.RoleId, x => x.BannedTime, x => x.BannedRemark);

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _userRepository.SaveChanges();

            return Task.CompletedTask;
        }

        public Task UpdateProfileAsync(UpdateProfileModel model, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Save new avatar
            var avatarImageModel = _imageRepository.SaveImage(model.AvatarFile);

            // Update information and avatar to profile
            var profile = new ProfileEntity
            {
                Id = LoggedInUser.Current.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                AvatarId = avatarImageModel?.Id
            };

            profile.FirstNameNorm = StringHelper.Normalize(profile.FirstName);
            profile.LastNameNorm = StringHelper.Normalize(profile.LastName);
            profile.FullName = string.Join(" ", profile.FirstName, profile.LastName);
            profile.FullNameNorm = StringHelper.Normalize(profile.FullName);

            if (profile.AvatarId != null)
            {
                _profileRepository.Update(profile,
                    x => x.LastName,
                    x => x.LastNameNorm,
                    x => x.FirstName,
                    x => x.FirstNameNorm,
                    x => x.FullName,
                    x => x.FullNameNorm,
                    x => x.AvatarId);
            }
            else
            {
                _profileRepository.Update(profile,
                    x => x.LastName,
                    x => x.LastNameNorm,
                    x => x.FirstName,
                    x => x.FirstNameNorm,
                    x => x.FullName,
                    x => x.FullNameNorm);
            }

            // Check cancellation token
            cancellationToken.ThrowIfCancellationRequested();

            _profileRepository.SaveChanges();

            // Remove old avatar
            if (LoggedInUser.Current.AvatarId.HasValue && avatarImageModel != null)
            {
                _imageRepository.RemoveImage(LoggedInUser.Current.AvatarId.Value);
            }

            return Task.CompletedTask;
        }

        public void CheckExistByEmail(string email)
        {
            var emailNorm = StringHelper.Normalize(email);

            var isExist = _userRepository.Get(x => x.EmailNorm == emailNorm).Any();

            if (!isExist)
            {
                throw new MonkeyException(ErrorCode.UserEmailNotExist);
            }
        }
    }
}