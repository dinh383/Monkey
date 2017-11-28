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
using Monkey.Core.Entities.ActivityLog;
using Newtonsoft.Json;
using Puppy.Core.DateTimeUtils;
using Puppy.EF;
using Puppy.EF.Interfaces;
using Puppy.EF.Repositories;
using Puppy.Web.Models;
using System;
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

            SaveLogActivityJob(listEntryAdded, listEntryModified, listEntryDeleted);

            return result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            StandardizeEntities();

            SplitEntityEntry(out List<EntityEntry> listEntryAdded, out List<EntityEntry> listEntryModified, out List<EntityEntry> listEntryDeleted);

            int result = DbContext.SaveChanges(acceptAllChangesOnSuccess);

            SaveLogActivityJob(listEntryAdded, listEntryModified, listEntryDeleted);

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            StandardizeEntities();

            SplitEntityEntry(out List<EntityEntry> listEntryAdded, out List<EntityEntry> listEntryModified, out List<EntityEntry> listEntryDeleted);

            var result = await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

            SaveLogActivityJob(listEntryAdded, listEntryModified, listEntryDeleted);

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            StandardizeEntities();

            SplitEntityEntry(out List<EntityEntry> listEntryAdded, out List<EntityEntry> listEntryModified, out List<EntityEntry> listEntryDeleted);

            var result = await DbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(true);

            SaveLogActivityJob(listEntryAdded, listEntryModified, listEntryDeleted);

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
                    entity.CreatedTime = DateTimeHelper.ReplaceNullOrDefault(entity.CreatedTime, dateTimeNow);
                    entity.CreatedBy = entity.CreatedBy ?? LoggedInUser.Current?.Id;
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

        #region Save Log Activity

        protected void SaveLogActivityJob(List<EntityEntry> listEntryAdded, List<EntityEntry> listEntryModified, List<EntityEntry> listEntryDeleted)
        {
            if (listEntryAdded?.Any() != true && listEntryModified?.Any() != true && listEntryDeleted?.Any() != true)
            {
                return;
            }

            List<ActivityLogEntity> listActivityLog = new List<ActivityLogEntity>();

            var dateTimeNow = DateTimeOffset.UtcNow;

            // Get activity log for added entity direct from entry
            if (listEntryAdded?.Any() == true)
            {
                foreach (var entryAdded in listEntryAdded)
                {
                    var entity = entryAdded.Entity as TEntity;
                    if (entity == null) continue;

                    var activityLog = GetNewActivityLogEntity(entryAdded);
                    activityLog.ActivityType = ActivityType.Added;
                    activityLog.DataJson = JsonConvert.SerializeObject(entity, Puppy.Core.Constants.StandardFormat.JsonSerializerSettings);

                    activityLog.CreatedTime = entity.CreatedTime;
                    activityLog.CreatedBy = entity.CreatedBy;

                    listActivityLog.Add(activityLog);
                }
            }

            // Get activity log for modified entity from database
            if (listEntryModified?.Any() == true)
            {
                var listModifiedActivityLog = listEntryModified.Select(GetNewActivityLogEntity).ToList();
                var listModifiedEntityId = listModifiedActivityLog.Select(x => x.Id).ToList();

                var listEntityAddedModified = Get(x => listModifiedEntityId.Contains(x.Id), true).ToList();

                foreach (var activityLog in listModifiedActivityLog)
                {
                    var entity = listEntityAddedModified.Single(x => x.Id == activityLog.Id);
                    activityLog.Id = entity.Id;
                    activityLog.GlobalId = entity.GlobalId;
                    activityLog.ActivityType = entity.DeletedTime == null ? ActivityType.Modified : ActivityType.SoftDeleted;
                    activityLog.DataJson = JsonConvert.SerializeObject(entity, Puppy.Core.Constants.StandardFormat.JsonSerializerSettings);

                    if (activityLog.ActivityType == ActivityType.Modified)
                    {
                        activityLog.LastUpdatedTime = entity.LastUpdatedTime ?? dateTimeNow;
                        activityLog.LastUpdatedBy = entity.LastUpdatedBy;
                    }
                    else
                    {
                        activityLog.DeletedTime = entity.DeletedTime ?? entity.LastUpdatedTime ?? dateTimeNow;
                        activityLog.DeletedBy = entity.DeletedBy ?? entity.LastUpdatedBy;
                    }

                    listActivityLog.Add(activityLog);
                }
            }

            // Get activity log for modified entity direct from entry (id, version and deleted time/by)
            foreach (var entryDeleted in listEntryDeleted)
            {
                var activityLog = GetNewActivityLogEntity(entryDeleted);
                activityLog.ActivityType = ActivityType.PhysicalDeleted;
                activityLog.DataJson = null;

                activityLog.DeletedTime = activityLog.DeletedTime ?? dateTimeNow;  // Keep source DeletedTime/now due to database data row gone.
                activityLog.DeletedBy = activityLog.DeletedBy;  // Keep source DeletedBy due to database data row gone.

                listActivityLog.Add(activityLog);
            }

            // 3. Call Background Job to Save log activities
            BackgroundJob.Enqueue(() => SaveLogActivity(listActivityLog.ToArray()));
        }

        public void SaveLogActivity(params ActivityLogEntity[] activityLogs)
        {
            foreach (var activityLog in activityLogs)
            {
                var activityLogAsJson = JsonConvert.SerializeObject(activityLog, Puppy.Core.Constants.StandardFormat.JsonSerializerSettings);
                Puppy.Logger.Log.Warning(activityLogAsJson);
            }
        }

        protected static ActivityLogEntity GetNewActivityLogEntity(EntityEntry entry)
        {
            var entity = (TEntity)entry.Entity;

            HttpContextInfoModel httpContextInfoModel = null;

            if (System.Web.HttpContext.Current != null)
            {
                httpContextInfoModel = new HttpContextInfoModel(System.Web.HttpContext.Current);
            }

            ActivityLogEntity activityLog = new ActivityLogEntity
            {
                Group = entry.Context.Model.FindEntityType(typeof(TEntity)).SqlServer().TableName,
                DataJson = null,
                HttpContextInfo = httpContextInfoModel,

                Id = entity.Id,
                GlobalId = entity.GlobalId,

                CreatedTime = entity.CreatedTime,
                CreatedBy = entity.CreatedBy,
                LastUpdatedTime = entity.LastUpdatedTime,
                LastUpdatedBy = entity.LastUpdatedBy,
                DeletedTime = entity.DeletedTime,
                DeletedBy = entity.DeletedBy
            };
            return activityLog;
        }

        #endregion
    }
}