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

using Monkey.Data.Interfaces;
using Puppy.DependencyInjection.Attributes;

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
    }
}