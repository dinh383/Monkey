#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository </Project>
//     <File>
//         <Name> RoleRepository.cs </Name>
//         <Created> 13/09/17 11:39:47 PM </Created>
//         <Key> 026c160c-1236-446d-91af-b6f820e90d01 </Key>
//     </File>
//     <Summary>
//         RoleRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Entities.Auth;
using Monkey.Data.User;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Data.EF.Repositories.User
{
    [PerRequestDependency(ServiceType = typeof(IRoleRepository))]
    public class RoleRepository : EntityRepository<RoleEntity>, IRoleRepository
    {
        public RoleRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}