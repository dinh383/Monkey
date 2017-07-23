#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> EntityRepositoryElastic.cs </Name>
//         <Created> 10 May 17 10:18:46 AM </Created>
//         <Key> d1ac81b1-35cf-40c3-9fb7-4fe2ccdf46f3 </Key>
//     </File>
//     <Summary>
//         EntityRepositoryElastic.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Monkey.Core;
using Monkey.Data.Entities;
using Monkey.Data.Interfaces;
using Puppy.Core.EnvironmentUtils;
using Puppy.DependencyInjection.Attributes;
using Puppy.EF;
using Puppy.Elastic;
using Puppy.Elastic.ContextSearch;
using Puppy.Elastic.Model.SearchModel;
using Puppy.Elastic.Model.SearchModel.Filters;
using Puppy.Elastic.Model.SearchModel.Queries;
using Puppy.Elastic.Model.Units;
using Puppy.Elastic.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Monkey.Data.EF
{
    [PerResolveDependency]
    public class EntityRepositoryElastic<TEntity, TElastic> : EntityRepository<TEntity>, ISearchableRepository<TElastic>
        where TEntity : BaseEntity, IBaseEntity where TElastic : class, IBaseElastic<int>
    {
        private readonly IConfigurationRoot _configurationRoot;

        internal EntityRepositoryElastic(IDbContext dbContext, IConfigurationRoot configurationRoot) : base(dbContext)
        {
            _configurationRoot = configurationRoot;
        }

        #region Elastic Connection

        private string _elasticConnectionString;

        internal string ElasticConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_elasticConnectionString))
                    _elasticConnectionString = _configurationRoot.GetValue<string>("Elastic:DatabaseConnectionString");
                return _elasticConnectionString;
            }
        }

        private readonly IElasticMappingResolver _elasticMappingResolver = new ElasticMappingResolver();

        public ElasticSerializerConfiguration Config => new ElasticSerializerConfiguration(_elasticMappingResolver);

        public bool? IsExistElasticIndex { get; private set; }

        #endregion

        #region Elastic

        public virtual void InitElasticMap()
        {
            if (IsExistElasticMap()) return;

            using (var context = new ElasticContext(ElasticConnectionString, Config))
            {
                if (!EnvironmentHelper.IsProduction())
                {
                    context.TraceProvider = new ConsoleTraceProvider();
                }
                context.IndexCreate<TElastic>();
            }
        }

        public virtual void ReInitElasticMap()
        {
            using (var context = new ElasticContext(ElasticConnectionString, Config))
            {
                if (!EnvironmentHelper.IsProduction())
                {
                    context.TraceProvider = new ConsoleTraceProvider();
                }

                if (IsExistElasticMap())
                {
                    context.DeleteIndex<TElastic>();
                }
                context.IndexCreate<TElastic>();
            }
        }

        public virtual bool IsExistElasticMap()
        {
            using (var context = new ElasticContext(ElasticConnectionString, Config))
            {
                if (!EnvironmentHelper.IsProduction())
                {
                    context.TraceProvider = new ConsoleTraceProvider();
                }
                IsExistElasticIndex = context.IndexExists<TElastic>();
                return IsExistElasticIndex == true;
            }
        }

        public virtual void ReInitAllElastic(int pageSize = Constants.ElasticSearch.MaxTakeRecord)
        {
            var skip = 0;
            var take = pageSize;
            var total = Get().Count();
            while (skip < total)
            {
                var listElastic =
                    Get()
                        .ProjectTo<TElastic>()
                        .Skip(skip)
                        .Take(take)
                        .ToList();

                AddUpdateElastic(listElastic);
                skip = skip + take;
            }
        }

        public virtual void AddUpdateElastic(IEnumerable<TElastic> listElastic)
        {
            using (var context = new ElasticContext(ElasticConnectionString, Config))
            {
                if (!EnvironmentHelper.IsProduction())
                {
                    context.TraceProvider = new ConsoleTraceProvider();
                }

                foreach (var elastic in listElastic)
                    context.AddUpdateDocument(elastic, elastic.Id);

                context.SaveChanges();
            }
        }

        public virtual void DeleteElastic(IEnumerable<int> listElasticId)
        {
            using (var context = new ElasticContext(ElasticConnectionString, Config))
            {
                if (!EnvironmentHelper.IsProduction())
                {
                    context.TraceProvider = new ConsoleTraceProvider();
                }

                foreach (var elasticId in listElasticId)
                {
                    context.DeleteDocument<TElastic>(elasticId);
                }

                context.SaveChanges();
            }
        }

        public void DeleteElasticMap()
        {
            using (var context = new ElasticContext(ElasticConnectionString, Config))
            {
                if (!EnvironmentHelper.IsProduction())
                {
                    context.TraceProvider = new ConsoleTraceProvider();
                }

                context.AllowDeleteForIndex = true;

                if (!IsExistElasticMap())
                {
                    return;
                }

                context.DeleteIndex<TElastic>();
                IsExistElasticIndex = false;
            }
        }

        public virtual IEnumerable<TElastic> GetElastic(out int total, int skip, int take = Constants.ElasticSearch.MaxTakeRecord)
        {
            var search = new Search
            {
                Query = new Query(new MatchAllQuery()),
                From = skip,
                Size = take
            };

            using (var context = new ElasticContext(ElasticConnectionString, Config))
            {
                var searchResult = context.Search<TElastic>(search);
                var listElastic = searchResult.PayloadResult?.Hits?.HitsResult?.Select(x => x.Source);
                total = searchResult.PayloadResult?.Hits?.Total ?? 0;
                return listElastic;
            }
        }

        public virtual IEnumerable<TElastic> GetElastic(string fieldName, List<object> listValue)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                throw new ArgumentException($"{nameof(fieldName)} is null or white space.");
            }

            listValue = listValue?.Where(x => x != null).ToList();

            if (listValue == null || !listValue.Any())
            {
                throw new ArgumentException($"{nameof(listValue)} is null or empty.");
            }

            fieldName = fieldName.ToLower();

            var search = new Search
            {
                Query = new Query(
                    new Filtered(
                        new Filter(
                            // filter by value
                            new TermsFilter(fieldName, listValue.ToList())))
                    {
                        Query = new Query(new MatchAllQuery())
                    })
            };

            var listElastic = new List<TElastic>();

            using (var context = new ElasticContext(ElasticConnectionString, Config))
            {
                var scanScrollConfig =
                    new ScanAndScrollConfiguration(new TimeUnitMinute(1), Constants.ElasticSearch.MaxTakeRecord);
                var searchResult = context.SearchCreateScanAndScroll<TElastic>(search, scanScrollConfig);
                var scrollId = searchResult.PayloadResult?.ScrollId;

                var total = searchResult.PayloadResult?.Hits?.Total ?? 0;
                var processedResults = 0;
                while (total > processedResults)
                {
                    var resultCollection = context.SearchScanAndScroll<TElastic>(scrollId, scanScrollConfig);
                    scrollId = resultCollection.PayloadResult?.ScrollId;

                    listElastic.AddRange(resultCollection.PayloadResult?.Hits?.HitsResult?.Select(x => x.Source));
                    processedResults = listElastic.Count;
                }

                return listElastic;
            }
        }

        public virtual bool IsExistElastic(object id)
        {
            using (var context = new ElasticContext(ElasticConnectionString, Config))
            {
                bool isExist = context.DocumentExists<TElastic>(id);
                return isExist;
            }
        }

        public virtual TElastic GetElastic(object id)
        {
            using (var context = new ElasticContext(ElasticConnectionString, Config))
            {
                var elastic = context.GetDocument<TElastic>(id);
                return elastic;
            }
        }

        #endregion

        #region Override Save Change: handle for Elastic, Caching

        /// <summary>
        ///     Get Add Update Delete Entity before call save change 
        /// </summary>
        /// <remarks> Only run when Exist Elastic </remarks>
        public virtual void GetAddUpdateDeleteEntity(out List<TEntity> listEntityAddUpdate, out List<TEntity> listEntityDelete)
        {
            listEntityAddUpdate = DbContext.ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified))
                .Select(x => x.Entity as TEntity).Where(x => x != null).ToList();

            listEntityDelete = DbContext.ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && x.State == EntityState.Deleted).Select(x => x.Entity as TEntity)
                .Where(x => x != null).ToList();
        }

        public virtual void SaveElastics<T>(List<T> listEntityAddUpdate, List<T> listEntityDelete) where T : BaseEntity, IBaseEntity
        {
            InitElasticMap();

            if (listEntityAddUpdate != null && listEntityAddUpdate.Any())
            {
                // Also Delete case if IsDeleted == true
                var listElasticDeletedId = listEntityAddUpdate.Where(x => x.IsDeleted).Select(x => x.Id).ToList();
                if (listElasticDeletedId?.Any() == true)
                {
                    DeleteElastic(listElasticDeletedId);
                    listEntityAddUpdate = listEntityAddUpdate.Where(x => !listElasticDeletedId.Contains(x.Id)).ToList();
                }

                // Add or Update
                var listElastic = Mapper.Map<List<TElastic>>(listEntityAddUpdate);
                AddUpdateElastic(listElastic);
            }

            if (listEntityDelete != null && listEntityDelete.Any())
            {
                var listElasticDeletedId = listEntityDelete.Select(x => x.Id);
                DeleteElastic(listElasticDeletedId);
            }
        }

        public override int SaveChanges()
        {
            StandardizeEntities();
            GetAddUpdateDeleteEntity(out List<TEntity> listEntityAddUpdate, out List<TEntity> listEntityDelete);
            int result = base.SaveChanges();
            BackgroundJob.Enqueue(() => SaveElastics(listEntityAddUpdate, listEntityDelete));
            return result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            StandardizeEntities();
            GetAddUpdateDeleteEntity(out List<TEntity> listEntityAddUpdate, out List<TEntity> listEntityDelete);
            int result = base.SaveChanges(acceptAllChangesOnSuccess);
            BackgroundJob.Enqueue(() => SaveElastics(listEntityAddUpdate, listEntityDelete));
            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            StandardizeEntities();
            GetAddUpdateDeleteEntity(out List<TEntity> listEntityAddUpdate, out List<TEntity> listEntityDelete);
            Task<int> result = base.SaveChangesAsync(cancellationToken);
            BackgroundJob.Enqueue(() => SaveElastics(listEntityAddUpdate, listEntityDelete));
            return result;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            StandardizeEntities();
            GetAddUpdateDeleteEntity(out List<TEntity> listEntityAddUpdate, out List<TEntity> listEntityDelete);
            Task<int> result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            BackgroundJob.Enqueue(() => SaveElastics(listEntityAddUpdate, listEntityDelete));
            return result;
        }

        #endregion
    }
}