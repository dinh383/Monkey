﻿#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> EntityRepository.cs </Name>
//         <Created> 02 May 17 12:52:21 PM </Created>
//         <Key> fe2d39e6-5f2e-4cfd-ad5f-8f588be696fe </Key>
//     </File>
//     <Summary>
//         EntityRepository.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Puppy.EF;
using Puppy.EF.Interfaces.Entity;

namespace Monkey.Data.EF
{
    public class EntityRepository<TEntity, TKey> : Puppy.EF.EntityRepository<TEntity, TKey> where TEntity : Entity, ISoftDeletableEntity<TKey>, IAuditableEntity<TKey> where TKey : struct
    {
        internal IDbContext DbContext { get; }

        internal EntityRepository(IDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }
    }
}