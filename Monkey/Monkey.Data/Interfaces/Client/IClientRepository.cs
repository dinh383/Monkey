#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository Interface </Project>
//     <File>
//         <Name> IClientRepository.cs </Name>
//         <Created> 14/09/17 7:53:14 PM </Created>
//         <Key> 6b4c1443-c0fb-473f-80a9-e744a17a9efa </Key>
//     </File>
//     <Summary>
//         IClientRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Data.Entities.Client;

namespace Monkey.Data.Interfaces.Client
{
    public interface IClientRepository : IEntityRepository<ClientEntity>
    {
    }
}