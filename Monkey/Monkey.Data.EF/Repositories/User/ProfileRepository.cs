#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository </Project>
//     <File>
//         <Name> ProfileRepository.cs </Name>
//         <Created> 13/09/17 11:39:57 PM </Created>
//         <Key> 1701e86f-be49-4c08-bb87-d88e924f61ce </Key>
//     </File>
//     <Summary>
//         ProfileRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Entities.User;
using Monkey.Data.User;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Data.EF.Repositories.User
{
    [PerRequestDependency(ServiceType = typeof(IProfileRepository))]
    public class ProfileRepository : EntityRepository<ProfileEntity>, IProfileRepository
    {
        public ProfileRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}