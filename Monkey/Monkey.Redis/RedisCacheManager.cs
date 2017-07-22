#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> RedisCacheManager.cs </Name>
//         <Created> 02 May 17 7:53:18 PM </Created>
//         <Key> 2a645ff7-b65d-47f9-9874-437ad2d21c06 </Key>
//     </File>
//     <Summary>
//         RedisCacheManager.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Microsoft.Extensions.Caching.Distributed;
using Monkey.Core;
using Puppy.DependencyInjection.Attributes;

namespace Monkey.Redis
{
    [SingletonDependency(ServiceType = typeof(ICacheManager))]
    public class RedisCacheManager : Puppy.Core.CacheUtils.RedisDistributedCacheHelper, ICacheManager
    {
        public RedisCacheManager(IDistributedCache cache) : base(cache)
        {
        }
    }
}