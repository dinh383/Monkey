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
using Monkey.Core.Entities.User;
using Monkey.Core.Exceptions;
using Monkey.Core.Models.User;
using Monkey.Data.User;
using Puppy.AutoMapper;
using Puppy.Core.StringUtils;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Business.Logic
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

        public Task<int> GetTotalAsync()
        {
            return _userRepository.Get().CountAsync();
        }

        // CHECK

        public void CheckExists(params int[] ids)
        {
            ids = ids.Distinct().ToArray();

            var totalInDb = _userRepository.Get(x => ids.Contains(x.Id)).Count();

            if (totalInDb != ids.Length)
            {
                throw new MonkeyException(ErrorCode.UserNotExist);
            }
        }

        public void CheckExists(params string[] userNames)
        {
            userNames = userNames.Distinct().Select(StringHelper.Normalize).ToArray();

            var totalInDb = _userRepository.Get(x => userNames.Contains(x.UserNameNorm)).Count();

            if (totalInDb != userNames.Length)
            {
                throw new MonkeyException(ErrorCode.UserNameNotExist);
            }
        }

        public void CheckActives(params string[] userNames)
        {
            userNames = userNames.Distinct().Select(StringHelper.Normalize).ToArray();

            var totalInDb = _userRepository.Get(x => userNames.Contains(x.UserNameNorm) && x.ActiveTime != null).Count();

            if (totalInDb != userNames.Length)
            {
                throw new MonkeyException(ErrorCode.UserInActive);
            }
        }

        public void CheckExistsBySubject(params string[] globalIds)
        {
            globalIds = globalIds.Distinct().ToArray();

            var totalInDb = _userRepository.Get(x => globalIds.Contains(x.GlobalId)).Count();

            if (totalInDb != globalIds.Length)
            {
                throw new MonkeyException(ErrorCode.UserNotExist);
            }
        }

        // CREATE

        public Task<string> CreateAsync(string email)
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

        // ACTIVE

        public async Task ActiveByEmailAsync(string globalId, string userName, string passwordHash, DateTimeOffset updatedTime)
        {
            var userEntity = await _userRepository.Get(x => x.GlobalId == globalId).SingleAsync().ConfigureAwait(true);
            userEntity.ActiveTime = userEntity.EmailConfirmedTime = DateTimeOffset.UtcNow;

            userEntity.UserName = userName;
            userEntity.UserNameNorm = StringHelper.Normalize(userName);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordLastUpdatedTime = updatedTime;

            _userRepository.Update(userEntity, x => x.UserName, x => x.UserNameNorm, x => x.ActiveTime,
                x => x.PhoneConfirmedTime, x => x.PasswordLastUpdatedTime,
                x => x.PasswordHash);

            _userRepository.SaveChanges();
        }

        public async Task ActiveByPhoneAsync(string globalId, string userName, string passwordHash, DateTimeOffset updatedTime)
        {
            var userEntity = await _userRepository.Get(x => x.GlobalId == globalId).SingleAsync().ConfigureAwait(true);
            userEntity.ActiveTime = userEntity.PhoneConfirmedTime = DateTimeOffset.UtcNow;

            userEntity.UserName = userName;
            userEntity.UserNameNorm = StringHelper.Normalize(userName);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordLastUpdatedTime = updatedTime;

            _userRepository.Update(userEntity, x => x.UserName, x => x.UserNameNorm, x => x.ActiveTime,
                x => x.PhoneConfirmedTime, x => x.PasswordLastUpdatedTime,
                x => x.PasswordHash);

            _userRepository.SaveChanges();
        }

        // GET

        public async Task<LoggedInUserModel> GetUserInfoBySubjectAsync(string globalId)
        {
            var user = await _userRepository.Get(x => x.GlobalId == globalId)
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

        public async Task<LoggedInUserModel> GetUserSubjectByRefreshTokenAsync(string refreshToken)
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
    }
}