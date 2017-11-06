using Microsoft.AspNetCore.SignalR;
using Monkey.Auth.Filters;
using Monkey.Core;
using Puppy.Core.DictionaryUtils;
using Puppy.DependencyInjection.Attributes;
using System;
using System.Collections.Concurrent;
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
        ///     Dictionary role id and List Connection Id 
        /// </summary>
        public static ConcurrentDictionary<int, ConcurrentDictionary<string, string>> ConnectedRoles = new ConcurrentDictionary<int, ConcurrentDictionary<string, string>>();

        public override Task OnConnectedAsync()
        {
            LoggedInUserBinder.BindLoggedInUser(Context.Connection.GetHttpContext());

            if (LoggedInUser.Current == null)
            {
                return base.OnConnectedAsync();
            }

            // Add user to connected users
            ConnectedUsers.AddOrUpdate(LoggedInUser.Current.Subject, Context.ConnectionId);

            // Add user to connected roles
            if (LoggedInUser.Current.RoleId == null)
            {
                return base.OnConnectedAsync();
            }

            if (!ConnectedRoles.TryGetValue(LoggedInUser.Current.RoleId.Value, out var connectedUsersInRole))
            {
                connectedUsersInRole = new ConcurrentDictionary<string, string>();
            }

            connectedUsersInRole.AddOrUpdate(LoggedInUser.Current.Subject, Context.ConnectionId);

            ConnectedRoles.AddOrUpdate(LoggedInUser.Current.RoleId.Value, connectedUsersInRole);

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

            if (LoggedInUser.Current.RoleId == null)
            {
                return base.OnDisconnectedAsync(ex);
            }

            if (!ConnectedRoles.TryGetValue(LoggedInUser.Current.RoleId.Value, out var connectedUsersInRole))
            {
                return base.OnDisconnectedAsync(ex);
            }

            connectedUsersInRole?.TryRemove(LoggedInUser.Current.Subject, out string _);

            ConnectedRoles.AddOrUpdate(LoggedInUser.Current.RoleId.Value, connectedUsersInRole);

            return base.OnDisconnectedAsync(ex);
        }

        public Task NotificationUserAsync(string subject, string message)
        {
            return !ConnectedUsers.TryGetValue(subject, out var connectionId)
                ? Task.CompletedTask
                : Clients.Client(connectionId).InvokeAsync(ClientMethodName, message);
        }

        public async Task NotificationRoleAsync(int roleId, string message)
        {
            if (!ConnectedRoles.TryGetValue(roleId, out var connectedUsers))
            {
                return;
            }

            if (connectedUsers?.Any() != true)
            {
                return;
            }

            foreach (var connectedUser in connectedUsers)
            {
                var connectionId = connectedUser.Value;
                await Clients.Client(connectionId).InvokeAsync(ClientMethodName, message).ConfigureAwait(true);
            }
        }

        public Task NotificationAllAsync(string message)
        {
            return Clients.All.InvokeAsync(ClientMethodName, message);
        }
    }
}