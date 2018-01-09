using Microsoft.AspNetCore.SignalR;
using Monkey.Core.Models.Notification.Portal;
using Monkey.Service.User;
using Puppy.DependencyInjection.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Areas.Portal.Hubs
{
    [SingletonDependency]
    public class NotificationHub : HubBase
    {
        private readonly IUserService _userService;

        public const string Url = "portal/notification-hub";
        public const string RequestRefreshClientMethodName = "refreshNotification";
        public const string SetNotificationsClientMethodName = "setNotification";

        public NotificationHub(IUserService userService)
        {
            _userService = userService;
        }

        public async Task RequestRefreshAsync(params string[] subjects)
        {
            List<string> connectionIds = GetListConnectionIds(subjects);

            if (connectionIds?.Any() != true)
            {
                return;
            }

            foreach (var connectionId in connectionIds)
            {
                await Clients.Client(connectionId).InvokeAsync(RequestRefreshClientMethodName).ConfigureAwait(true);
            }
        }

        /// <summary>
        ///     Receive all notification of current logged in user 
        /// </summary>
        /// <returns></returns>
        public async Task GetAllAsync()
        {
            var connectionId = GetLoggedInUserConnectionId();

            if (string.IsNullOrWhiteSpace(connectionId))
            {
                return;
            }

            List<NotificationPortalModel> notifications = new List<NotificationPortalModel>();  // TODO Implement Later

            foreach (var notification in notifications)
            {
                SetUrl(notification);
            }

            await Clients.Client(connectionId).InvokeAsync(SetNotificationsClientMethodName, notifications).ConfigureAwait(true);
        }

        /// <summary>
        ///     Set URL for notification base on Type 
        /// </summary>
        /// <param name="notification"></param>
        private static void SetUrl(NotificationPortalModel notification)
        {
            // TODO Implement Later
        }
    }
}