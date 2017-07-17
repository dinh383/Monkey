using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monkey.Data.EF.Factory;
using System.Reflection;

namespace Monkey
{
    public partial class Startup
    {
        public static class Database
        {
            public static void Service(IServiceCollection services)
            {
                services.AddDbContext<DbContext>(
                    builder => builder.UseSqlServer(ConfigurationRoot.GetConnectionString(Environment.EnvironmentName),
                        options => options.MigrationsAssembly(typeof(IDataModule).GetTypeInfo().Assembly.GetName()
                            .Name)));
            }
        }
    }
}