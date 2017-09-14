#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository Interface </Project>
//     <File>
//         <Name> IRoleRepository.cs </Name>
//         <Created> 13/09/17 11:39:25 PM </Created>
//         <Key> 7fdfe122-1f56-4fc2-a8d0-2458772b6bb7 </Key>
//     </File>
//     <Summary>
//         IRoleRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Data.Entities.User;

namespace Monkey.Data.Interfaces.User
{
    public interface IRoleRepository : IEntityRepository<RoleEntity>
    {
    }
}