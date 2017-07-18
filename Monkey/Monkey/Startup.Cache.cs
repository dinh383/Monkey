using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Monkey
{
    public partial class Startup
    {
        public static class Cache
        {
            public static void Service(IServiceCollection services)
            {
                services.AddSingleton<IDistributedCache>(factory =>
                {
                    var cache = new RedisCache(new RedisCacheOptions
                    {
                        Configuration = Core.SystemConfigs.Redis.ConnectionString,
                        InstanceName = Core.SystemConfigs.Redis.InstanceName
                    });

                    return cache;
                });
            }
        }
    }
}