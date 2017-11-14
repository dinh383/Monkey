using Microsoft.AspNetCore.SignalR;
using Monkey.Auth.Filters;
using Monkey.Core;
using Monkey.Core.Constants;
using Puppy.Core.DictionaryUtils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monkey.Areas.Portal.Hubs
{
    public abstract class HubBase : Hub
    {
        /// <summary>
        ///     Dictionary user Subject/Global Id and ConnectionId 
        /// </summary>
        protected static readonly ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

        /// <summary>
        ///     Dictionary permission and List Connection Id 
        /// </summary>
        protected static readonly ConcurrentDictionary<Enums.Permission, ConcurrentDictionary<string, string>> ConnectedPermissions = new ConcurrentDictionary<Enums.Permission, ConcurrentDictionary<string, string>>();

        protected virtual List<string> GetListConnectionIds(params string[] subjects)
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

        protected virtual List<string> GetListConnectionIds(params Enums.Permission[] permissions)
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

        protected virtual string GetLoggedInUserConnectionId()
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
    }
}