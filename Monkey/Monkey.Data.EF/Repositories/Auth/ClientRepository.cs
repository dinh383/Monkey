#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository </Project>
//     <File>
//         <Name> ClientRepository.cs </Name>
//         <Created> 14/09/17 7:57:33 PM </Created>
//         <Key> 26db13f4-99f4-4302-b98f-492bb628a536 </Key>
//     </File>
//     <Summary>
//         ClientRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Entities.Auth;
using Monkey.Data.Client;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Data.EF.Repositories.Auth
{
    [PerRequestDependency(ServiceType = typeof(IClientRepository))]
    public class ClientRepository : EntityRepository<ClientEntity>, IClientRepository
    {
        public ClientRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}