using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Monkey.Core.Constants;
using Monkey.Core.Entities;
using Puppy.Core.ObjectUtils;
using Puppy.DependencyInjection.Attributes;
using Puppy.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monkey.Data.EF.Repositories
{
    [PerRequestDependency(ServiceType = typeof(IConfigurationRepository))]
    public class ConfigurationRepository : EntityRepository<ConfigurationEntity>, IConfigurationRepository
    {
        private const string KeyPrefix = nameof(ConfigurationRepository);

        private readonly DistributedCacheEntryOptions _cacheOption = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromDays(30)
        };

        private readonly IRedisCacheManager _cacheManager;

        public ConfigurationRepository(IDbContext dbContext, IRedisCacheManager cacheManager) : base(dbContext)
        {
            _cacheManager = cacheManager;
        }

        /// <inheritdoc />
        public T Get<T>(Configuration.Key key)
        {
            var data = GetListByKeys(key).Select(x => x.Value).Single();
            return data.ConvertTo<T>();
        }

        /// <inheritdoc />
        public Dictionary<Configuration.Key, string> GetByKeys(params Configuration.Key[] keys)
        {
            keys = keys.Distinct().ToArray();
            var listData = GetListByKeys(keys).Select(x => new { x.Id, x.Value }).ToList();
            var listResult = listData.ToDictionary(x => (Configuration.Key)x.Id, x => x.Value);
            return listResult;
        }

        /// <inheritdoc />
        public Dictionary<Configuration.Key, string> GetByGroups(params Configuration.KeyGroup[] groups)
        {
            var keys = groups.SelectMany(Configuration.GetKeys).Distinct().ToArray();
            var listData = GetListByKeys(keys).Select(x => new { x.Id, x.Value }).ToList();
            var listResult = listData.ToDictionary(x => (Configuration.Key)x.Id, x => x.Value);
            return listResult;
        }

        public void ReBuildCache()
        {
            BuildCache();
        }

        #region Cache Helper Methods

        private IEnumerable<ConfigurationEntity> GetListByKeys(params Configuration.Key[] keys)
        {
            List<ConfigurationEntity> listData = new List<ConfigurationEntity>();
            foreach (var key in keys)
            {
                string cacheKey = $"{KeyPrefix}_{(int)key}";

                if (!_cacheManager.IsExist(cacheKey))
                {
                    BuildCache();
                }

                var data = _cacheManager.Get<ConfigurationEntity>(cacheKey);
                listData.Add(data);
            }

            return listData;
        }

        private void BuildCache()
        {
            var allData = Get().ToList();

            foreach (var configurationEntity in allData)
            {
                string key = $"{KeyPrefix}_{configurationEntity.Id}";
                _cacheManager.Set(key, configurationEntity, _cacheOption);
            }
        }

        public override void StandardizeEntities()
        {
            // Add, Update, Delete cache
            var listState = new List<EntityState>
            {
                EntityState.Added,
                EntityState.Modified,
                EntityState.Deleted
            };

            var listEntryAddUpdate = DbContext.ChangeTracker.Entries()
                .Where(x => x.Entity is ConfigurationEntity && listState.Contains(x.State))
                .Select(x => x).ToList();

            foreach (var entry in listEntryAddUpdate)
            {
                var entity = entry.Entity as ConfigurationEntity;

                if (entity == null)
                    continue;

                string key = $"{KeyPrefix}_{entity.Id}";

                if (entry.State == EntityState.Added)
                {
                    // Add
                    _cacheManager.Set(key, entity, _cacheOption);
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entity.DeletedTime != null)
                    {
                        // Delete
                        _cacheManager.Remove(key);
                    }
                    else
                    {
                        // Update
                        _cacheManager.Set(key, entity, _cacheOption);
                    }
                }
                else
                {
                    // Delete
                    _cacheManager.Remove(key);
                }
            }
        }

        #endregion Cache Helper Methods
    }
}