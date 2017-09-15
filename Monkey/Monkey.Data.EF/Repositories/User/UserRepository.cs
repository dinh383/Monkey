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

using Monkey.Core.Entities.User;
using Monkey.Data.User;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Data.EF.Repositories.User
{
    [PerRequestDependency(ServiceType = typeof(IUserRepository))]
    public class UserRepository : EntityRepository<UserEntity>, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}