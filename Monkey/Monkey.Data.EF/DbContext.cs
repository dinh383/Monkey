using Microsoft.EntityFrameworkCore;
using Monkey.Data.EF.Factory;
using Puppy.DependencyInjection.Attributes;
using Puppy.EF;
using Puppy.EF.Mapping;

namespace Monkey.Data.EF
{
    [PerResolveDependency(ServiceType = typeof(IDbContext))]
    public sealed partial class DbContext : BaseDbContext, IDbContext
    {
        /// <summary>
        ///     Set CMD timeout is 20 minutes 
        /// </summary>
        public readonly int CmdTimeoutInSecond = 12000;

        public DbContext()
        {
            Database.SetCommandTimeout(CmdTimeoutInSecond);
        }

        public DbContext(DbContextOptions options) : base(options)
        {
            Database.SetCommandTimeout(CmdTimeoutInSecond);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbContextFactory.GetConnectionString(), o => o.MigrationsAssembly(DbContextFactory.GetMigrationAssemblyName()));
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // [Important] Keep Under Base For Override And Make End Result

            // Scan and apply Config/Mapping for Tables/Entities (from folder "Map")
            builder.AddConfigFromAssembly(DbContextFactory.GetMigrationAssembly());

            // Set Delete Behavior as Restrict in Relationship
            builder.DisableCascadingDelete();

            // Convention for Table name
            builder.RemovePluralizingTableNameConvention();

            builder.ReplaceTableNameConvention("Entity", string.Empty);
        }
    }
}