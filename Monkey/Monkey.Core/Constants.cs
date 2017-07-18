#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Author> Top </Author>
//     <Project> Monkey.Auth.Domain </Project>
//     <File>
//         <Name> Constants </Name>
//         <Created> 08/04/2017 11:31:32 PM </Created>
//         <Key> dc620a3e-88e2-4916-b998-b44e6c48db03 </Key>
//     </File>
//     <Summary>
//         Constants
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Monkey.Core
{
    /// <summary>
    ///     System Setting/Constant like <c>appsetting.json</c> but this information <c>very important and not allow</c>
    ///     for Edit in run-time
    /// </summary>
    public static class Constants
    {
        public static class Setting
        {
            public const string WebRoot = "Assets";

            public const string CookieSchemaName = "Monkey_Cookie";

            public static class Cros
            {
                public const string PolicyAllowAll = "CrosPolicyAllowAll";
            }

            public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public static class ElasticSearch
        {
            /// <summary>
            ///     Maximum record take when use elastic, do not change this 
            /// </summary>
            public const int MaxTakeRecord = 10000;
        }

        /// <summary>
        ///     Use for Global and common information 
        /// </summary>
        public static class Cache
        {
            /// <summary>
            ///     Key name for cache all data 
            /// </summary>
            public static class KeyName
            {
            }

            /// <summary>
            ///     Sliding cache 30 days 
            /// </summary>
            public static DistributedCacheEntryOptions DefaultSlidingOption = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(30)
            };

            /// <summary>
            ///     Cache absolute 1 day 
            /// </summary>
            public static DistributedCacheEntryOptions DefaultAbsoluteOption = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow + TimeSpan.FromDays(1)
            };
        }

        /// <summary>
        ///     Use only Plural Noun for Endpoint 
        /// </summary>
        public static class ApiEndPoints
        {
            public const string Base = "api";
            public const string Root = ".well-known/" + Base;
        }
    }
}