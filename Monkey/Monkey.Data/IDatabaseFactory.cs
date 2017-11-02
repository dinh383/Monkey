#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey → Interface </Project>
//     <File>
//         <Name> IDatabaseFactory.cs </Name>
//         <Created> 25/07/17 3:22:36 PM </Created>
//         <Key> 3d02b07f-5adb-467b-aaa9-8a8f3246406a </Key>
//     </File>
//     <Summary>
//         IDatabaseFactory.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Builder;
using System;

namespace Monkey.Data
{
    public interface IDatabaseFactory
    {
        /// <summary>
        ///     <para>
        ///         Applies any pending migrations for the context to the database. Will create the
        ///         database if it does not already exist.
        ///     </para>
        /// </summary>
        IServiceProvider MigrateDatabase(IServiceProvider services);

        /// <summary>
        ///     <para>
        ///         Applies any pending migrations for the context to the database. Will create the
        ///         database if it does not already exist.
        ///     </para>
        /// </summary>
        IApplicationBuilder MigrateDatabase(IApplicationBuilder app);
    }
}