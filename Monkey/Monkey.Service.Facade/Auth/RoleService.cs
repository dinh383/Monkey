#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Service Facade </Project>
//     <File>
//         <Name> RoleService.cs </Name>
//         <Created> 06/10/17 10:55:41 PM </Created>
//         <Key> d2444d72-83fc-4090-b922-45fad8a28ab3 </Key>
//     </File>
//     <Summary>
//         RoleService.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Business.Auth;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Service.Facade.Auth
{
    [PerRequestDependency(ServiceType = typeof(IRoleService))]
    public class RoleService : IRoleService
    {
        private readonly IRoleBusiness _roleBusiness;

        public RoleService(IRoleBusiness roleBusiness)
        {
            _roleBusiness = roleBusiness;
        }
    }
}