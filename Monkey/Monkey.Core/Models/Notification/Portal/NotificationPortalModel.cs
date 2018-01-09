using Monkey.Core.Constants;
using System;

namespace Monkey.Core.Models.Notification.Portal
{
    public class NotificationPortalModel
    {
        public string GroupId { get; set; }

        public int Id { get; set; }

        public bool IsRead => ReadTime != null;

        public DateTimeOffset? ReadTime { get; set; }

        public Enums.NotificationType Type { get; set; } = Enums.NotificationType.Information;

        public string Message { get; set; }

        public DateTimeOffset CreatedTime { get; set; } = SystemUtils.SystemTimeNow;

        public string Url { get; set; }

        public string Data { get; set; }
    }
}