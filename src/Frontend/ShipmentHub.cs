namespace Frontend
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public class ShipmentHub : Hub
    {
        public static List<string> ConnectedUsers;

        public override Task OnConnected()
        {
            if (ConnectedUsers == null)
                ConnectedUsers = new List<string>();

            ConnectedUsers.Add(Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            ConnectedUsers.Remove(Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}
