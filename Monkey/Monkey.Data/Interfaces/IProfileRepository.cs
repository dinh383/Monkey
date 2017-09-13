#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository Interface </Project>
//     <File>
//         <Name> IProfileRepository.cs </Name>
//         <Created> 13/09/17 11:38:22 PM </Created>
//         <Key> 363da610-84c4-4bad-9c48-745e187c96c4 </Key>
//     </File>
//     <Summary>
//         IProfileRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Data.Entities.User;

namespace Monkey.Data.Interfaces
{
    public interface IProfileRepository : IEntityRepository<ProfileEntity>
    {
    }
}