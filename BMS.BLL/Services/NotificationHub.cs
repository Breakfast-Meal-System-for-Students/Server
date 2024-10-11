using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Services
{
    public class NotificationHub : Hub
    {

        private static readonly ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                OnlineUsers[userId] = Context.ConnectionId;
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                OnlineUsers.TryRemove(userId, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public static bool TryGetConnectionId(string username, out string connectionId)
        {
            return OnlineUsers.TryGetValue(username, out connectionId);
        }
    }
}
