#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> DatabaseExtensions.cs </Name>
//         <Created> 31/07/17 10:40:23 PM </Created>
//         <Key> f8df6a21-bea1-4540-8e46-74485e78d57b </Key>
//     </File>
//     <Summary>
//         DatabaseExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Monkey.Data.EF.Factory;
using Puppy.Core.TypeUtils;

namespace Monkey.Extensions
{
    public static class DatabaseExtensions
    {
        /// <summary>
        ///     [Database] Use Entity Framework 
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services
                .AddDbContext<DbContext>(builder =>
                    builder
                        .UseSqlServer(Core.SystemConfigs.DatabaseConnectionString,
                            options => options.MigrationsAssembly(typeof(IDatabase).GetAssemblySimpleName())));
            return services;
        }
    }
}