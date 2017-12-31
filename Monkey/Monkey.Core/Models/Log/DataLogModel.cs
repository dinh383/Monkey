using System;
using Monkey.Core.Entities.Log;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Puppy.Web.Models;

namespace Monkey.Core.Models.Log
{
    public class DataLogModel
    {
        /// <summary>
        ///     Log Id, the rest information define for source data 
        /// </summary>
        public string LogId { get; set; } = Guid.NewGuid().ToString("N");

        [JsonConverter(typeof(StringEnumConverter))]
        public DataLogType LogType { get; set; }

        public HttpContextInfoModel HttpContextInfo { get; set; }

        /// <summary>
        ///     Table Name in Source Database 
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        ///     Source Data 
        /// </summary>
        /// <remarks> Null in case LogType is Soft or Physical Deleted </remarks>
        public object Data { get; set; }

        public int Id { get; set; }

        public string GlobalId { get; set; } = Guid.NewGuid().ToString("N");

        public DateTimeOffset CreatedTime { get; set; } = SystemUtils.SystemTimeNow;

        public int? CreatedBy { get; set; }

        public DateTimeOffset? LastUpdatedTime { get; set; }

        public int? LastUpdatedBy { get; set; }

        public int? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}