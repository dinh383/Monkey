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
using Puppy.EF.Interfaces.Entity;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Data.EF
{
    public class EntityRepository<TEntity, TKey> : Puppy.EF.EntityRepository<TEntity, TKey> where TEntity : Entity, ISoftDeletableEntity<TKey>, IAuditableEntity<TKey> where TKey : struct
    {
        internal IDbContext DbContext { get; }

        internal EntityRepository(IDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public void StandardizeEntities()
        {
            var listEntryAddUpdate = DbContext.ChangeTracker.Entries()
                .Where(x => x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified))
                .Select(x => x).ToList();

            foreach (var entry in listEntryAddUpdate)
            {
                var entity = entry.Entity as TEntity;

                if (entity == null)
                    continue;

                if (entry.State == EntityState.Added)
                {
                    entity.IsDeleted = false;
                    entity.LastUpdatedTime = null;
                    entity.CreatedTime = entity.CreatedTime == default(DateTimeOffset)
                        ? SystemUtils.GetSystemTimeNow()
                        : entity.CreatedTime;
                }
                else
                {
                    if (entity.IsDeleted)
                        entity.DeletedTime = entity.DeletedTime == default(DateTimeOffset)
                            ? SystemUtils.GetSystemTimeNow()
                            : entity.DeletedTime;
                    else
                        entity.LastUpdatedTime = entity.LastUpdatedTime == default(DateTimeOffset)
                            ? SystemUtils.GetSystemTimeNow()
                            : entity.LastUpdatedTime;
                }
            }
        }

        [DebuggerStepThrough]
        public override int SaveChanges()
        {
            StandardizeEntities();
            return base.SaveChanges();
        }

        [DebuggerStepThrough]
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            StandardizeEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        [DebuggerStepThrough]
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            StandardizeEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        [DebuggerStepThrough]
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            StandardizeEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}