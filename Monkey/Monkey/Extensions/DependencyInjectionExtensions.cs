#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> DependencyInjectionExtensions.cs </Name>
//         <Created> 31/07/17 10:40:38 PM </Created>
//         <Key> b50e5a57-7e56-4555-baf9-9951b2758480 </Key>
//     </File>
//     <Summary>
//         DependencyInjectionExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Puppy.DependencyInjection;
using System.IO;

namespace Monkey.Extensions
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        ///     [Dependency Injection] 
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddDependencyInjectionMonkey(this IServiceCollection services)
        {
            var systemPrefix = $"{nameof(Monkey)}";
            services
                .AddDependencyInjectionScanner()
                .ScanFromAllAssemblies($"{systemPrefix}.*.dll", Path.GetFullPath(PlatformServices.Default.Application.ApplicationBasePath));

            // Write out all dependency injection services
            services.WriteOut(systemPrefix);

            return services;
        }
    }
}