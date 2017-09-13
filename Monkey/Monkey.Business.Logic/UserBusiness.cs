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

using Monkey.Core.Exceptions;
using Monkey.Data.Interfaces;
using Puppy.DependencyInjection.Attributes;
using System.Linq;
using Puppy.Core.StringUtils;

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
                throw new MonkeyException(ErrorCode.UserNotExist);
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
    }
}