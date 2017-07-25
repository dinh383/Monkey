#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Facade </Project>
//     <File>
//         <Name> UserService.cs </Name>
//         <Created> 18/07/17 4:54:23 PM </Created>
//         <Key> 4e04746d-98a2-4f10-a6c6-b9a485ef0e44 </Key>
//     </File>
//     <Summary>
//         UserService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Business;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Service.Facade
{
    [PerRequestDependency(ServiceType = typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IUserBusiness _userBusiness;

        public UserService(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }
    }
}