#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> Provider.cs </Name>
//         <Created> 02 May 17 7:53:18 PM </Created>
//         <Key> 2a645ff7-b65d-47f9-9874-437ad2d21c06 </Key>
//     </File>
//     <Summary>
//         Provider.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Monkey.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Text;

namespace Monkey.CacheProvider.Redis
{
    [SingletonDependency(ServiceType = typeof(ICacheManager))]
    public class Provider : ICacheManager
    {
        protected IDistributedCache Cache;

        public Provider(IDistributedCache cache)
        {
            Cache = cache;
        }

        /// <summary>
        ///     Absolute Expiration by duration 
        /// </summary>
        /// <param name="key">     </param>
        /// <param name="data">    </param>
        /// <param name="duration"></param>
        public void Set(string key, object data, TimeSpan duration)
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
                return;

            Set(key, data, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow + duration
            });
        }

        public void Set(string key, object data, DistributedCacheEntryOptions options)
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
                return;

            var dataStr = data as string;
            var dataStore = dataStr ?? JsonConvert.SerializeObject(data);
            Cache.Set(key, Encoding.UTF8.GetBytes(dataStore), options);
        }

        public void Remove(string key)
        {
            if (IsExist(key))
                Cache.Remove(key);
        }

        public bool IsExist(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            var fromCache = Cache.Get(key);
            return fromCache != null;
        }

        public T Get<T>(string key) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            var fromCache = Cache.Get(key);
            if (fromCache == null)
                return null;

            var str = Encoding.UTF8.GetString(fromCache);
            if (typeof(T) == typeof(string))
                return str as T;

            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}