using Microsoft.EntityFrameworkCore;
using Monkey.Core.LogEntities;
using Monkey.Data.EF.Factory;
using Puppy.DependencyInjection.Attributes;
using Puppy.EF;
using Puppy.EF.Extensions;

namespace Monkey.Data.EF
{
    [PerResolveDependency(ServiceType = typeof(ILogDbContext))]
    public sealed class LogDbContext : BaseDbContext, ILogDbContext
    {
        /// <summary>
        ///     Set CMD timeout is 20 minutes 
        /// </summary>
        public readonly int CmdTimeoutInSecond = 12000;

        public DbSet<DataLogEntity> DataLogs { get; set; }

        public LogDbContext()
        {
            Database.SetCommandTimeout(CmdTimeoutInSecond);
        }

        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {
            Database.SetCommandTimeout(CmdTimeoutInSecond);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                LogDbContextFactory.GetLogDbContextBuilder(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // [Important] Keep Under Base For Override And Make End Result

            builder.AddConfigFromAssembly<LogDbContext>(DbContextFactory.GetMigrationAssembly());

            // Set Delete Behavior as Restrict in Relationship
            builder.DisableCascadingDelete();

            // Convention for Table name
            builder.RemovePluralizingTableNameConvention();

            builder.ReplaceTableNameConvention("Entity", string.Empty);
        }
    }
}