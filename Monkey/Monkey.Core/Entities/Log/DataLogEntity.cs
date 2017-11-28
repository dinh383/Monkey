using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Puppy.Web.Models;

namespace Monkey.Core.Entities.Log
{
    public class DataLogEntity : Entity
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
    }
}