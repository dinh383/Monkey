using Microsoft.AspNetCore.SignalR;
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

        public const string Url = "hub/portal/notification";
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

            object pagedCollectionNotifications = null;

            //var pagedCollectionNotifications = await _userService.GetNotificationsAsync(new PagedCollectionParametersModel { Skip = 0, Take = 10 }, true).ConfigureAwait(true);

            await Clients.Client(connectionId).InvokeAsync(SetNotificationsClientMethodName, pagedCollectionNotifications).ConfigureAwait(true);
        }
    }
}