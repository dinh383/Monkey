using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Monkey.Data.EF.Factory;
using Puppy.DependencyInjection.Attributes;
using Puppy.EF;
using Puppy.EF.Mapping;
using System;
using System.Linq;
using System.Reflection;

namespace Monkey.Data.EF
{
    [PerResolveDependency(ServiceType = typeof(IDbContext))]
    public sealed partial class DbContext : BaseDbContext, IDbContext
    {
        private readonly int cmdTimeoutInSecond = 12000; // 20 mins

        public DbContext()
        {
            Database.SetCommandTimeout(cmdTimeoutInSecond);
        }

        public DbContext(DbContextOptions options) : base(options)
        {
            Database.SetCommandTimeout(cmdTimeoutInSecond);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();
                var connectionString = config.GetSection($"ConnectionStrings:{environmentName}").Value;

                optionsBuilder.UseSqlServer(connectionString,
                    o => o.MigrationsAssembly(typeof(IDatabase).GetTypeInfo().Assembly.GetName().Name));
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            // Keep under base for override and make end result
            builder.AddConfigFromAssembly(typeof(IDatabase).GetTypeInfo().Assembly);
        }
    }
}