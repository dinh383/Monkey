#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository Interface </Project>
//     <File>
//         <Name> IPermissionRepository.cs </Name>
//         <Created> 13/09/17 11:40:12 PM </Created>
//         <Key> 24e24875-10a5-4dc4-be79-cdbd93e293c2 </Key>
//     </File>
//     <Summary>
//         IPermissionRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Core.Entities.Auth;

namespace Monkey.Data.Auth
{
    public interface IPermissionRepository : IEntityRepository<PermissionEntity>
    {
    }
}