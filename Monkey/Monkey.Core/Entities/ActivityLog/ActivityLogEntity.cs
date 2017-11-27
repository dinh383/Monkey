using Newtonsoft.Json;
using Puppy.Web.Models;
using System;

namespace Monkey.Core.Entities.ActivityLog
{
    /// <summary>
    ///     Activity Log own properties: LogId, Group (Source Table Name), DataJson (Source JSON
    ///     String) and Activity Type (Added / Modified / SoftDeleted / PhysicalDeleted)
    /// </summary>
    public class ActivityLogEntity : Entity
    {
        public string LogId { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        ///     Table Name in Source Database 
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        ///     Source JSON String 
        /// </summary>
        /// <remarks> Null in case ActivityType is Soft or Physical Deleted </remarks>
        public string DataJson { get; set; }

        #region Http Context Info

        private HttpContextInfoModel _httpContextInfo;

        public HttpContextInfoModel HttpContextInfo
        {
            get => _httpContextInfo;
            set
            {
                _httpContextInfo = value;
                _httpContextJson = _httpContextInfo?.ToString();
            }
        }

        private string _httpContextJson;

        public string HttpContextJson
        {
            get => _httpContextJson;
            set
            {
                _httpContextInfo = string.IsNullOrWhiteSpace(value) ? null : JsonConvert.DeserializeObject<HttpContextInfoModel>(value);
                _httpContextJson = value;
            }
        }

        #endregion Http Context Info

        #region Activity Type

        private ActivityType _activityType = ActivityType.Modified;

        /// <summary>
        ///     Activity Type (Added / Modified / SoftDeleted / PhysicalDeleted) 
        /// </summary>
        [JsonIgnore]
        public ActivityType ActivityType
        {
            get => _activityType;
            set
            {
                _activityTypeStr = value.ToString();
                _activityType = value;
            }
        }

        private string _activityTypeStr = ActivityType.Modified.ToString();

        public string ActivityTypeStr
        {
            get => _activityTypeStr;
            set
            {
                _activityType = (ActivityType)Enum.Parse(typeof(ActivityType), value, true);
                _activityTypeStr = value;
            }
        }

        #endregion Activity Type
    }
}