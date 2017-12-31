using Monkey.Core.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Monkey.Core.Models.Notification.Portal
{
    public class NotificationPortalModel
    {
        public int Id { get; set; }

        public bool IsRead { get; set; }

        public Enums.NotificationType Type { get; set; }

        public string Message { get; set; }

        public DateTimeOffset CreatedTime { get; set; } = SystemUtils.SystemTimeNow;

        public string Url { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> AdditionalData { get; set; } = new Dictionary<string, object>();
    }
}