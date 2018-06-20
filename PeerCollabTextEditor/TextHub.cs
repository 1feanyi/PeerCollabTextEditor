using Microsoft.AspNetCore.SignalR;
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerCollabTextEditor
{
    // Hub controls the communication between clients.
    public class TextHub : Hub
    {
        private static Dictionary<string, string> connectionsNgroup = new Dictionary<string, string>();

        // Handles when a new connection is started by adding connection to a group
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        // Removes a connection from its group when the connection is terminated both un/expectedly
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (connectionsNgroup.ContainsKey(Context.ConnectionId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connectionsNgroup[Context.ConnectionId]);
                connectionsNgroup.Remove(Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        // Handles text based communication between the clients by 
        // sending text that it receives to all other clients in the same group
        public async Task BroadcastText(string text)
        {
            if (connectionsNgroup.ContainsKey(Context.ConnectionId))
            {
                await Clients.OthersInGroup(connectionsNgroup[Context.ConnectionId]).SendAsync("ReceiveText", text);
            }
        }

        // Takes group as parameter and checks for an existing membership of another group
        // SignalR doesn't provide a way of viewing which connections are in each group
        public async Task JoinGroup(string group)
        {
            if (connectionsNgroup.ContainsKey(Context.ConnectionId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connectionsNgroup[Context.ConnectionId]);
                connectionsNgroup.Remove(Context.ConnectionId);
            }
            connectionsNgroup.Add(Context.ConnectionId, group);
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
    }
}
