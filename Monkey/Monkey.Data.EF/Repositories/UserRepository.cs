#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository </Project>
//     <File>
//         <Name> UserRepository.cs </Name>
//         <Created> 18/07/17 4:43:29 PM </Created>
//         <Key> a571a4e9-19ee-470e-94af-d392beddf95b </Key>
//     </File>
//     <Summary>
//         UserRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Data.Entities;
using Monkey.Data.Interfaces;
using Puppy.DependencyInjection.Attributes;
using Puppy.EF;

namespace Monkey.Data.EF.Repositories
{
    [PerRequestDependency(ServiceType = typeof(IUserRepository))]
    public class UserRepository : Puppy.EF.EntityRepository<UserEntity, int>, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}