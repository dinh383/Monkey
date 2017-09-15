#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> IEntityRepository.cs </Name>
//         <Created> 09/09/17 6:20:42 PM </Created>
//         <Key> 1887a50d-ef7b-46e8-8e67-f262458d2411 </Key>
//     </File>
//     <Summary>
//         IEntityRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Puppy.EF.Interfaces.Repositories;
using Entity = Monkey.Core.Entities.Entity;

namespace Monkey.Data
{
    public interface IEntityRepository<TEntity> : IEntityRepository<TEntity, int> where TEntity : Entity, new()
    {
    }
}