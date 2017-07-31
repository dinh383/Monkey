#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> ServiceCollectionExtensions.cs </Name>
//         <Created> 27/07/17 1:26:59 AM </Created>
//         <Key> 18da2c44-f356-4e52-b761-b9f453d13e90 </Key>
//     </File>
//     <Summary>
//         ServiceCollectionExtensions.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Monkey.Core;
using Monkey.Core.ConfigModels;
using System;

namespace Monkey.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRedisCache(this IServiceCollection services)
        {
            services.AddSingleton<IDistributedCache>(factory =>
            {
                var cache = new RedisCache(new RedisCacheOptions
                {
                    Configuration = SystemConfigs.DistributedCache.ConnectionString,
                    InstanceName = SystemConfigs.DistributedCache.InstanceName
                });

                return cache;
            });
        }

        public static void AddRedisCache(this IServiceCollection services, Action<DistributedCacheConfigModel> options)
        {
            DistributedCacheConfigModel config = (DistributedCacheConfigModel)options.Target;
            services.AddSingleton<IDistributedCache>(factory =>
            {
                var cache = new RedisCache(new RedisCacheOptions
                {
                    Configuration = config.ConnectionString,
                    InstanceName = config.InstanceName
                });

                return cache;
            });
        }
    }
}