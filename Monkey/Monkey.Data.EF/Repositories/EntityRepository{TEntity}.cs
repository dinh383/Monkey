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

using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Monkey.Core;
using Monkey.Core.Configs;
using Monkey.Core.Entities.DataLog;
using Puppy.Core.DateTimeUtils;
using Puppy.Core.ObjectUtils;
using Puppy.DependencyInjection;
using Puppy.EF;
using Puppy.EF.Interfaces;
using Puppy.EF.Repositories;
using Puppy.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Data.EF.Repositories
{
    public class EntityRepository<TEntity> : EntityRepository<TEntity, int> where TEntity : Entity, new()
    {
        protected EntityRepository(IBaseDbContext dbContext) : base(dbContext)
        {
        }

        #region Save Change

        public override int SaveChanges()
        {
            StandardizeEntities();

            SplitEntityEntry(out List<EntityEntry> listEntryAdded, out List<EntityEntry> listEntryModified, out List<EntityEntry> listEntryDeleted);

            int result = DbContext.SaveChanges();

            if (SystemConfig.IsUseLogDatabase)
            {
                SaveDataLogJob(listEntryAdded, listEntryModified, listEntryDeleted);
            }

            return result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            StandardizeEntities();

            SplitEntityEntry(out List<EntityEntry> listEntryAdded, out List<EntityEntry> listEntryModified, out List<EntityEntry> listEntryDeleted);

            int result = DbContext.SaveChanges(acceptAllChangesOnSuccess);

            if (SystemConfig.IsUseLogDatabase)
            {
                SaveDataLogJob(listEntryAdded, listEntryModified, listEntryDeleted);
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            StandardizeEntities();

            SplitEntityEntry(out List<EntityEntry> listEntryAdded, out List<EntityEntry> listEntryModified, out List<EntityEntry> listEntryDeleted);

            var result = await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            if (SystemConfig.IsUseLogDatabase)
            {
                SaveDataLogJob(listEntryAdded, listEntryModified, listEntryDeleted);
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            StandardizeEntities();

            SplitEntityEntry(out List<EntityEntry> listEntryAdded, out List<EntityEntry> listEntryModified, out List<EntityEntry> listEntryDeleted);

            var result = await DbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(true);

            if (SystemConfig.IsUseLogDatabase)
            {
                SaveDataLogJob(listEntryAdded, listEntryModified, listEntryDeleted);
            }

            return result;
        }

        public override void StandardizeEntities()
        {
            var listState = new List<EntityState>
            {
                EntityState.Added,
                EntityState.Modified
            };

            var listEntryAddUpdate = DbContext.ChangeTracker.Entries()
                .Where(x => x.Entity is TEntity && listState.Contains(x.State))
                .Select(x => x).ToList();

            var dateTimeNow = SystemUtils.SystemTimeNow;

            foreach (var entry in listEntryAddUpdate)
            {
                if (!(entry.Entity is TEntity entity))
                {
                    continue;
                }

                if (entry.State == EntityState.Added)
                {
                    entity.DeletedTime = null;
                    entity.LastUpdatedTime = entity.CreatedTime = DateTimeHelper.ReplaceNullOrDefault(entity.CreatedTime, dateTimeNow);
                    entity.LastUpdatedBy = entity.CreatedBy = entity.CreatedBy ?? LoggedInUser.Current?.Id;
                }
                else
                {
                    if (entity.DeletedTime != null)
                    {
                        entity.DeletedTime = DateTimeHelper.ReplaceNullOrDefault(entity.DeletedTime, dateTimeNow);
                        entity.DeletedBy = entity.DeletedBy ?? LoggedInUser.Current?.Id;
                    }
                    else
                    {
                        entity.LastUpdatedTime = DateTimeHelper.ReplaceNullOrDefault(entity.LastUpdatedTime, dateTimeNow);
                        entity.LastUpdatedBy = entity.LastUpdatedBy ?? LoggedInUser.Current?.Id;
                    }
                }
            }
        }

        #endregion

        #region Save Data Log

        protected void SaveDataLogJob(List<EntityEntry> listEntryAdded, List<EntityEntry> listEntryModified, List<EntityEntry> listEntryDeleted)
        {
            if (listEntryAdded?.Any() != true && listEntryModified?.Any() != true && listEntryDeleted?.Any() != true)
            {
                return;
            }

            List<DataLogEntity> listDataLog = new List<DataLogEntity>();

            var dateTimeNow = SystemUtils.SystemTimeNow;

            // Get data log for added entity direct from entry

            if (listEntryAdded?.Any() == true)
            {
                foreach (var entryAdded in listEntryAdded)
                {
                    if (!(entryAdded.Entity is TEntity entity))
                    {
                        continue;
                    }

                    var dataLog = NewDataLog(entryAdded);

                    dataLog.DataJson = entity.PreventReferenceLoop().ToJsonString();
                    dataLog.DataCreatedTime = entity.CreatedTime;
                    dataLog.DataCreatedBy = entity.CreatedBy;

                    dataLog.LogType = DataLogType.Added;

                    // Standardize Data Log
                    dataLog.LogCreatedTime = dateTimeNow;
                    dataLog.LogCreatedBy = LoggedInUser.Current?.Id;

                    listDataLog.Add(dataLog);
                }
            }

            // Get data log for modified entity from database
            if (listEntryModified?.Any() == true)
            {
                var listModifiedDataLog = listEntryModified.Select(NewDataLog).ToList();

                var listModifiedEntityId = listModifiedDataLog.Select(x => x.DataId).ToList();

                var listEntityAddedModified = Get(x => listModifiedEntityId.Contains(x.Id), true).ToList();

                foreach (var dataLog in listModifiedDataLog)
                {
                    var entity = listEntityAddedModified.Single(x => x.Id == dataLog.DataId);

                    dataLog.DataId = entity.Id;
                    dataLog.DataGlobalId = entity.GlobalId;
                    dataLog.DataJson = entity.PreventReferenceLoop().ToJsonString();

                    dataLog.DataCreatedTime = entity.CreatedTime;
                    dataLog.DataCreatedBy = entity.CreatedBy;

                    dataLog.DataLastUpdatedTime = entity.LastUpdatedTime;
                    dataLog.DataLastUpdatedBy = entity.LastUpdatedBy;

                    dataLog.DataDeletedTime = entity.DeletedTime;
                    dataLog.DataDeletedBy = entity.DeletedBy;

                    dataLog.LogType = entity.DeletedTime == null ? DataLogType.Modified : DataLogType.SoftDeleted;

                    // Standardize Data Log
                    dataLog.LogCreatedTime = dateTimeNow;
                    dataLog.LogCreatedBy = LoggedInUser.Current?.Id;

                    listDataLog.Add(dataLog);
                }
            }

            // Get data log for modified entity direct from entry (id, version and deleted time/by)
            foreach (var entryDeleted in listEntryDeleted)
            {
                var dataLog = NewDataLog(entryDeleted);

                dataLog.DataJson = null;

                dataLog.DataDeletedTime = dataLog.DataDeletedTime ?? dateTimeNow;  // Keep source DeletedTime/now due to database data row gone.

                dataLog.DataDeletedBy = dataLog.DataDeletedBy;  // Keep source DeletedBy due to database data row gone.

                dataLog.LogType = DataLogType.PhysicalDeleted;

                // Standardize Data Log
                dataLog.LogCreatedTime = dateTimeNow;
                dataLog.LogCreatedBy = LoggedInUser.Current?.Id;

                listDataLog.Add(dataLog);
            }

            // 3. Call Background Job to Save log activities
            BackgroundJob.Enqueue(() => SaveDataLog(listDataLog.ToArray()));
        }

        protected static DataLogEntity NewDataLog(EntityEntry entry)
        {
            var entity = (TEntity)entry.Entity;

            HttpContextInfoModel httpContextInfoModel = null;

            if (System.Web.HttpContext.Current != null)
            {
                httpContextInfoModel = new HttpContextInfoModel(System.Web.HttpContext.Current);
            }

            DataLogEntity dataLog = new DataLogEntity
            {
                LogHttpContextInfoJson = httpContextInfoModel?.ToString(),

                DataGroup = entry.Context.Model.FindEntityType(typeof(TEntity)).SqlServer().TableName,
                DataJson = null,
                DataId = entity.Id,
                DataGlobalId = entity.GlobalId,
                DataCreatedTime = entity.CreatedTime,
                DataCreatedBy = entity.CreatedBy,
                DataLastUpdatedTime = entity.LastUpdatedTime,
                DataLastUpdatedBy = entity.LastUpdatedBy,
                DataDeletedTime = entity.DeletedTime,
                DataDeletedBy = entity.DeletedBy
            };
            return dataLog;
        }

        public void SaveDataLog(params DataLogEntity[] dataLogs)
        {
            if (!SystemConfig.IsUseLogDatabase)
            {
                return;
            }

            ILogDbContext logDbContext = Resolver.Resolve<ILogDbContext>();

            foreach (var dataLog in dataLogs)
            {
                logDbContext.Add(dataLog);
            }

            logDbContext.SaveChanges();
        }

        #endregion
    }
}