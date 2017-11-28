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

namespace Monkey.Data.EF.Factory
{
    public static class DatabaseExtensions
    {
        /// <summary>
        ///     [Database] Use Entity Framework 
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<DbContext>(builder => builder.UseSqlServer());

            return services;
        }

        /// <summary>
        ///     [Database] Use SQL Server with Migration and Use row no for paging 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseSqlServer(this DbContextOptionsBuilder builder)
        {
            var dbContextOptionsBuilder = (DbContextOptionsBuilder<DbContext>)builder;

            DbContextFactory.GetDbContextBuilder(dbContextOptionsBuilder);

            return builder;
        }
    }
}