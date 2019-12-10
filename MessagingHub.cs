using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace emo_back
{
    public static class ConnectedUsers
    {
        public static List<string> ConnectionIds { get; set; }
    }

    public class MessagingHub : Hub<IMessagingHubClient>
    {
        public override Task OnConnectedAsync()
        {
            string username = Context.User.Identity.Name ?? "Anonymous";
            ConnectedUsers.ConnectionIds.Add(username);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            string username = Context.User.Identity.Name;
            if (ConnectedUsers.ConnectionIds.Contains(username))
                ConnectedUsers.ConnectionIds.Remove(username);
            return base.OnDisconnectedAsync(exception);
        }
    }
}