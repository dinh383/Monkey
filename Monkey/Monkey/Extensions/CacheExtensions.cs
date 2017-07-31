#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> CacheExtensions.cs </Name>
//         <Created> 31/07/17 11:38:36 PM </Created>
//         <Key> fa0f418e-6fa5-44d4-b4ce-9b84d8e41ae9 </Key>
//     </File>
//     <Summary>
//         CacheExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.Extensions.DependencyInjection;
using Monkey.Redis;

namespace Monkey.Extensions
{
    public static class CacheExtensions
    {
        public static IServiceCollection AddCacheMonkey(this IServiceCollection services)
        {
            // In-memory cache
            services.AddMemoryCache();

            // Distributed cache by Redis
            services.AddRedisCache();

            return services;
        }
    }
}