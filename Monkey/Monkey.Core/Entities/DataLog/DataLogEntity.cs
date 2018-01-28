using System;

namespace Monkey.Core.Entities.DataLog
{
    public class DataLogEntity
    {
        public int LogId { get; set; }

        public string LogGlobalId { get; set; } = Guid.NewGuid().ToString("N");

        public DataLogType LogType { get; set; } = DataLogType.Added;

        public DateTimeOffset LogCreatedTime { get; set; } = SystemUtils.SystemTimeNow;

        public int? LogCreatedBy { get; set; } = LoggedInUser.Current?.Id;

        /// <summary>
        ///     Json serialize from <see cref="Puppy.Web.Models.HttpContextInfoModel" /> object 
        /// </summary>
        public string LogHttpContextInfoJson { get; set; }

        /// <summary>
        ///     Table Name in Source Database 
        /// </summary>
        public string DataGroup { get; set; }

        public int DataId { get; set; }

        public string DataGlobalId { get; set; }

        public DateTimeOffset DataCreatedTime { get; set; }

        public int? DataCreatedBy { get; set; }

        public DateTimeOffset DataLastUpdatedTime { get; set; }

        public int? DataLastUpdatedBy { get; set; }

        public DateTimeOffset? DataDeletedTime { get; set; }

        public int? DataDeletedBy { get; set; }

        /// <summary>
        ///     Json serialize from Source Data object 
        /// </summary>
        /// <remarks> Null in case LogType is Soft or Physical Deleted </remarks>
        public string DataJson { get; set; }
    }
}