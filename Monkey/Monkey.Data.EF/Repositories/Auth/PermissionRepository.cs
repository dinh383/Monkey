#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository </Project>
//     <File>
//         <Name> PermissionRepository.cs </Name>
//         <Created> 13/09/17 11:40:23 PM </Created>
//         <Key> a43b8779-27bd-411b-b519-df8e3144673d </Key>
//     </File>
//     <Summary>
//         PermissionRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Entities.Auth;
using Monkey.Data.Auth;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Data.EF.Repositories.Auth
{
    [PerRequestDependency(ServiceType = typeof(IPermissionRepository))]
    public class PermissionRepository : EntityRepository<PermissionEntity>, IPermissionRepository
    {
        public PermissionRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}