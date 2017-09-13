#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Repository Interface </Project>
//     <File>
//         <Name> IRefreshTokenRepository.cs </Name>
//         <Created> 13/09/17 11:24:49 PM </Created>
//         <Key> 8fee94b9-a567-4aa7-8443-dfa90e71e0ed </Key>
//     </File>
//     <Summary>
//         IRefreshTokenRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Data.Entities.User;

namespace Monkey.Data.Interfaces
{
    public interface IRefreshTokenRepository : IEntityRepository<RefreshTokenEntity>
    {
    }
}