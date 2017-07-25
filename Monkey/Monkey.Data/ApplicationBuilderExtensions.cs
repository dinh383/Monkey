#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ApplicationBuilderExtensions.cs </Name>
//         <Created> 25/07/17 3:21:41 PM </Created>
//         <Key> c55b0450-7fef-4aaf-8022-5456245cc210 </Key>
//     </File>
//     <Summary>
//         ApplicationBuilderExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Monkey.Data
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///     <para>
        ///         Applies any pending migrations for the context to the database. Will create the
        ///         database if it does not already exist.
        ///     </para>
        /// </summary>
        public static IApplicationBuilder DatabaseMigrate(this IApplicationBuilder app)
        {
            Interfaces.IDatabaseFactory databaseFactory = app.ApplicationServices.GetService<Interfaces.IDatabaseFactory>();
            return databaseFactory.MigrateDatabase(app);
        }
    }
}