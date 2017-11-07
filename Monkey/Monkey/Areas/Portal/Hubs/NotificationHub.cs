using Microsoft.AspNetCore.SignalR;
using Monkey.Auth.Filters;
using Monkey.Core;
using Monkey.Core.Constants;
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

        public const string ClientMethodName = "notification";

        /// <summary>
        ///     Dictionary user Subject/Global Id and ConnectionId 
        /// </summary>
        public static ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

        /// <summary>
        ///     Dictionary permission and List Connection Id 
        /// </summary>
        public static ConcurrentDictionary<Enums.Permission, ConcurrentDictionary<string, string>> ConnectedPermissions = new ConcurrentDictionary<Enums.Permission, ConcurrentDictionary<string, string>>();

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

        public async Task NotificationUsersAsync(string message, params string[] subjects)
        {
            LoggedInUserBinder.BindLoggedInUser(Context.Connection.GetHttpContext());

            if (LoggedInUser.Current == null)
            {
                return;
            }

            subjects = subjects?.Distinct().ToArray();

            if (subjects?.Any() != true)
            {
                return;
            }

            List<string> connectionIds = new List<string>();

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

            if (connectionIds.Any() != true)
            {
                return;
            }

            foreach (var connectionId in connectionIds)
            {
                await Clients.Client(connectionId).InvokeAsync(ClientMethodName, message).ConfigureAwait(true);
            }
        }

        public async Task NotificationPermissionsAsync(string message, params Enums.Permission[] permissions)
        {
            LoggedInUserBinder.BindLoggedInUser(Context.Connection.GetHttpContext());

            if (LoggedInUser.Current == null)
            {
                return;
            }

            permissions = permissions?.Distinct().ToArray();

            if (permissions?.Any() != true)
            {
                return;
            }

            List<string> connectionIds = new List<string>();

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

            if (connectionIds.Any() != true)
            {
                return;
            }

            foreach (var connectionId in connectionIds)
            {
                await Clients.Client(connectionId).InvokeAsync(ClientMethodName, message).ConfigureAwait(true);
            }
        }

        public Task NotificationAllAsync(string message)
        {
            return Clients.All.InvokeAsync(ClientMethodName, message);
        }
    }
}