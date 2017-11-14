using Microsoft.AspNetCore.SignalR;
using Monkey.Core.Constants;
using Monkey.Core.Models.Notification.Portal;
using Puppy.DependencyInjection.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Areas.Portal.Hubs
{
    [SingletonDependency]
    public class NotificationHub : HubBase
    {
        public const string Url = "portal/notification";
        public const string AddNotificationClientMethodName = "addNotification";
        public const string SetNotificationsClientMethodName = "setNotification";

        /// <summary>
        ///     Send to users have subject in <paramref name="subjects" /> 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="subjects">    </param>
        /// <returns></returns>
        public async Task SendNotificationToSubjectsAsync(NotificationPortalModel notification, params string[] subjects)
        {
            List<string> connectionIds = GetListConnectionIds(subjects);

            if (connectionIds?.Any() != true)
            {
                return;
            }

            foreach (var connectionId in connectionIds)
            {
                await Clients.Client(connectionId).InvokeAsync(AddNotificationClientMethodName, notification).ConfigureAwait(true);
            }
        }

        /// <summary>
        ///     Send to users have any permission in <paramref name="permissions" /> 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="permissions"> </param>
        /// <returns></returns>
        public async Task SendNotificationToPermissionsAsync(NotificationPortalModel notification, params Enums.Permission[] permissions)
        {
            List<string> connectionIds = GetListConnectionIds(permissions);

            if (connectionIds?.Any() != true)
            {
                return;
            }

            foreach (var connectionId in connectionIds)
            {
                await Clients.Client(connectionId).InvokeAsync(AddNotificationClientMethodName, notification).ConfigureAwait(true);
            }
        }

        /// <summary>
        ///     Send to All 
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public Task SendNotificationToAllAsync(NotificationPortalModel notification)
        {
            return Clients.All.InvokeAsync(AddNotificationClientMethodName, notification);
        }

        /// <summary>
        ///     Receive all notification of current logged in user 
        /// </summary>
        /// <returns></returns>
        public Task ReceiveAllNotificationAsync()
        {
            var connectionId = GetLoggedInUserConnectionId();

            if (string.IsNullOrWhiteSpace(connectionId))
            {
                return Task.CompletedTask;
            }

            // TODO - Get list un-read notifications
            List<NotificationPortalModel> notifications = new List<NotificationPortalModel>();

            return Clients.Client(connectionId).InvokeAsync(SetNotificationsClientMethodName, notifications);
        }
    }
}