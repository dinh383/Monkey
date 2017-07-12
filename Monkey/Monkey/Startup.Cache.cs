using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
//using Puppy.Core.CacheUtils;

namespace Monkey
{
    public partial class Startup
    {
        public static class Cache
        {
            public static void Service(IServiceCollection services)
            {
                var redisConnection = ConfigurationRoot["Redis:ConnectionString"];
                var redisInstance = ConfigurationRoot["Redis:InstanceName"];

                services.AddSingleton<IDistributedCache>(factory =>
                {
                    var cache = new RedisCache(new RedisCacheOptions
                    {
                        Configuration = redisConnection,
                        InstanceName = redisInstance
                    });

                    return cache;
                });

                // Add Redis as IDistributedCacheManager Service
                //services.AddScoped<IDistributedCacheManager, RedisCacheManager>();
            }
        }
    }
}