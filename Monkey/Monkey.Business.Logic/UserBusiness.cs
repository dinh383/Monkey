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
using Monkey.Data.User;
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

        public UserBusiness(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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

        public void CheckExistsByGlobalId(params string[] globalIds)
        {
            globalIds = globalIds.Distinct().ToArray();

            var totalInDb = _userRepository.Get(x => globalIds.Contains(x.GlobalId)).Count();

            if (totalInDb != globalIds.Length)
            {
                throw new MonkeyException(ErrorCode.UserNotExist);
            }
        }

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

        public async Task ActiveByEmailAsync(string globalId, string userName, string passwordHash, string passwordSalt)
        {
            var userEntity = await _userRepository.Get(x => x.GlobalId == globalId).SingleAsync().ConfigureAwait(true);
            userEntity.ActiveTime = userEntity.EmailConfirmedTime = DateTimeOffset.UtcNow;

            userEntity.UserName = userName;
            userEntity.UserNameNorm = StringHelper.Normalize(userName);
            userEntity.PasswordSalt = passwordSalt;
            userEntity.PasswordHash = passwordHash;

            _userRepository.Update(userEntity, x => x.UserName, x => x.UserNameNorm, x => x.ActiveTime,
                x => x.PhoneConfirmedTime, x => x.PasswordSalt,
                x => x.PasswordHash);

            _userRepository.SaveChanges();
        }

        public async Task ActiveByPhoneAsync(string globalId, string userName, string passwordHash, string passwordSalt)
        {
            var userEntity = await _userRepository.Get(x => x.GlobalId == globalId).SingleAsync().ConfigureAwait(true);
            userEntity.ActiveTime = userEntity.PhoneConfirmedTime = DateTimeOffset.UtcNow;

            userEntity.UserName = userName;
            userEntity.UserNameNorm = StringHelper.Normalize(userName);
            userEntity.PasswordSalt = passwordSalt;
            userEntity.PasswordHash = passwordHash;

            _userRepository.Update(userEntity, x => x.UserName, x => x.UserNameNorm, x => x.ActiveTime,
                x => x.PhoneConfirmedTime, x => x.PasswordSalt,
                x => x.PasswordHash);

            _userRepository.SaveChanges();
        }

        public Task<int> GetTotalAsync()
        {
            return _userRepository.Get().CountAsync();
        }
    }
}