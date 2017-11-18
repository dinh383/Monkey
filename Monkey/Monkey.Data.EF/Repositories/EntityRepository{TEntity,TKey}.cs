#region	License

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

using Microsoft.EntityFrameworkCore;
using Monkey.Core;
using Puppy.EF;
using Puppy.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monkey.Data.EF.Repositories
{
    public class EntityRepository<TEntity> : EntityRepository<TEntity, int> where TEntity : Entity, new()
    {
        internal EntityRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public override void StandardizeEntities()
        {
            var listState = new List<EntityState>
            {
                EntityState.Added,
                EntityState.Modified
            };

            var listEntryAddUpdate = DbContext.ChangeTracker.Entries().Where(x => x.Entity is TEntity && listState.Contains(x.State)).Select(x => x).ToList();

            var dateTimeNow = DateTimeOffset.UtcNow;

            foreach (var entry in listEntryAddUpdate)
            {
                var entity = entry.Entity as TEntity;

                if (entity == null)
                    continue;

                if (entry.State == EntityState.Added)
                {
                    entity.DeletedTime = null;
                    entity.LastUpdatedTime = null;
                    entity.CreatedTime = dateTimeNow;
                    entity.CreatedBy = LoggedInUser.Current?.Id;
                }
                else
                {
                    if (entity.DeletedTime != null)
                    {
                        entity.DeletedTime = dateTimeNow;
                        entity.DeletedBy = LoggedInUser.Current?.Id;
                    }
                    else
                    {
                        entity.LastUpdatedTime = dateTimeNow;
                        entity.LastUpdatedBy = LoggedInUser.Current?.Id;
                    }
                }
            }
        }
    }
}