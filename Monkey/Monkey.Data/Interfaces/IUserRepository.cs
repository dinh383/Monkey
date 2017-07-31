#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Business Interface </Project>
//     <File>
//         <Name> IUserRepository.cs </Name>
//         <Created> 18/07/17 4:42:06 PM </Created>
//         <Key> 2aefeeb7-9f78-4f05-ba98-7e7fa11157fe </Key>
//     </File>
//     <Summary>
//         IUserRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Monkey.Data.Entities;
using Puppy.EF.Interfaces.Repository;

namespace Monkey.Data.Interfaces
{
    public interface IUserRepository : IEntityRepository<UserEntity>
    {
    }
}