using Microsoft.AspNetCore.SignalR;
using Monkey.Auth.Filters;
using Monkey.Core;
using Monkey.Core.Constants;
using Monkey.Core.Models.Notification.Portal;
using Puppy.Core.DictionaryUtils;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Areas.Portal.Hubs
{
    [SingletonDependency]
    public class NotificationHub : Hub
    {
        public const string Url = "portal/notification";

        public const string AddNotificationClientMethodName = "addNotification";
        public const string SetNotificationsClientMethodName = "setNotification";

        #region Connections

        /// <summary>
        ///     Dictionary user Subject/Global Id and ConnectionId 
        /// </summary>
        public static ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

        /// <summary>
        ///     Dictionary permission and List Connection Id 
        /// </summary>
        public static ConcurrentDictionary<Enums.Permission, ConcurrentDictionary<string, string>> ConnectedPermissions = new ConcurrentDictionary<Enums.Permission, ConcurrentDictionary<string, string>>();

        public List<string> GetListConnectionIds(params string[] subjects)
        {
            List<string> connectionIds = new List<string>();

            LoggedInUserBinder.BindLoggedInUser(Context.Connection.GetHttpContext());

            if (LoggedInUser.Current == null)
            {
                return connectionIds;
            }

            subjects = subjects?.Distinct().ToArray();

            if (subjects?.Any() != true)
            {
                return connectionIds;
            }

            foreach (var subject in subjects)
            {
                if (!ConnectedUsers.TryGetValue(subject, out var connectionId))
                {
                    continue;
                }

                if (!connectionIds.Contains(connectionId))
                {
                    connectionIds.Add(connectionId);
                }
            }

            return connectionIds;
        }

        public List<string> GetListConnectionIds(params Enums.Permission[] permissions)
        {
            List<string> connectionIds = new List<string>();

            LoggedInUserBinder.BindLoggedInUser(Context.Connection.GetHttpContext());

            if (LoggedInUser.Current == null)
            {
                return connectionIds;
            }

            permissions = permissions?.Distinct().ToArray();

            if (permissions?.Any() != true)
            {
                return connectionIds;
            }

            foreach (var permission in permissions)
            {
                if (!ConnectedPermissions.TryGetValue(permission, out var connectedUsers))
                {
                    continue;
                }

                if (connectedUsers?.Any() != true)
                {
                    continue;
                }

                foreach (var connectedUser in connectedUsers)
                {
                    var connectionId = connectedUser.Value;

                    if (!connectionIds.Contains(connectionId))
                    {
                        connectionIds.Add(connectionId);
                    }
                }
            }

            return connectionIds;
        }

        public string GetLoggedInUserConnectionId()
        {
            LoggedInUserBinder.BindLoggedInUser(Context.Connection.GetHttpContext());

            if (LoggedInUser.Current == null)
            {
                return null;
            }

            ConnectedUsers.TryGetValue(LoggedInUser.Current.Subject, out var connectionId);

            return connectionId;
        }

        public override Task OnConnectedAsync()
        {
            LoggedInUserBinder.BindLoggedInUser(Context.Connection.GetHttpContext());

            if (LoggedInUser.Current == null)
            {
                return base.OnConnectedAsync();
            }

            // Add user to connected users

            ConnectedUsers.AddOrUpdate(LoggedInUser.Current.Subject, Context.ConnectionId);

            // Add user to connected permissions

            if (LoggedInUser.Current.ListPermission?.Any() != true)
            {
                return base.OnConnectedAsync();
            }

            foreach (var permission in LoggedInUser.Current.ListPermission)
            {
                if (!ConnectedPermissions.TryGetValue(permission, out var connectedUsersHavePermission))
                {
                    connectedUsersHavePermission = new ConcurrentDictionary<string, string>();
                }

                connectedUsersHavePermission.AddOrUpdate(LoggedInUser.Current.Subject, Context.ConnectionId);

                ConnectedPermissions.AddOrUpdate(permission, connectedUsersHavePermission);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            LoggedInUserBinder.BindLoggedInUser(Context.Connection.GetHttpContext());

            if (LoggedInUser.Current == null)
            {
                return base.OnDisconnectedAsync(ex);
            }

            // Remove user from connected users

            ConnectedUsers.TryRemove(LoggedInUser.Current.Subject, out string _);

            // Remove user from connected roles

            if (LoggedInUser.Current.ListPermission?.Any() != true)
            {
                return base.OnConnectedAsync();
            }

            foreach (var permission in LoggedInUser.Current.ListPermission)
            {
                if (!ConnectedPermissions.TryGetValue(permission, out var connectedUsersHavePermission))
                {
                    continue;
                }

                connectedUsersHavePermission.TryRemove(LoggedInUser.Current.Subject, out string _);

                ConnectedPermissions.AddOrUpdate(permission, connectedUsersHavePermission);
            }

            return base.OnDisconnectedAsync(ex);
        }

        #endregion Connections

        #region Notification

        /// <summary>
        ///     Send to users have subject in <paramref name="subjects" /> 
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="subjects">    </param>
        /// <returns></returns>
        public async Task SendNotificationAsync(NotificationPortalModel notification, params string[] subjects)
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
        public async Task SendNotificationAsync(NotificationPortalModel notification, params Enums.Permission[] permissions)
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
        public Task SendNotificationAsync(NotificationPortalModel notification)
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

            // TODO - Get list un-read notifications
            List<NotificationPortalModel> notifications = new List<NotificationPortalModel>();

            return Clients.Client(connectionId).InvokeAsync(SetNotificationsClientMethodName, notifications);
        }

        #endregion Notification
    }
}