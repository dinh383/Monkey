#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> DaatabaseFactory.cs </Name>
//         <Created> 25/07/17 3:27:15 PM </Created>
//         <Key> 2c0f9c42-dee6-4234-8002-95e51d3c4cd8 </Key>
//     </File>
//     <Summary>
//         DaatabaseFactory.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.AspNetCore.Builder;
using Monkey.Core.Configs;
using Puppy.DependencyInjection.Attributes;
using Puppy.EF;
using System;

namespace Monkey.Data.EF.Factory
{
    [PerResolveDependency(ServiceType = typeof(IDatabaseFactory))]
    public class DatabaseFactory : IDatabaseFactory
    {
        public IServiceProvider MigrateDatabase(IServiceProvider services)
        {
            services.MigrateDatabase<IDbContext>();

            if (SystemConfig.IsUseLogDatabase)
            {
                services.MigrateDatabase<LogDbContext>();
            }

            return services;
        }

        public IApplicationBuilder MigrateDatabase(IApplicationBuilder app)
        {
            app.MigrateDatabase<IDbContext>();

            if (SystemConfig.IsUseLogDatabase)
            {
                app.MigrateDatabase<LogDbContext>();
            }

            return app;
        }
    }
}