namespace Frontend
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Newtonsoft.Json;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using RabbitMQ.Client.Events;

    public class ShipmentHub : Hub
    {
        public static List<string> ConnectedUsers;

        public ShipmentHub(IChannel channel)
        {
            Task.Run(() =>
            {
                channel.Listen(QueueConfiguration.SuccessQueue, ConsumerOnSuccess);
            });
        }

        private void ConsumerOnSuccess(object sender, BasicDeliverEventArgs e)
        {
            var body = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine("Received: " + body);
            var message = JsonConvert.DeserializeObject(body);

            var typeName = ((dynamic)message).Metadata.TypeName.ToString();
            var messageType = Type.GetType(typeName);
            var typedMessage = JsonConvert.DeserializeObject(body, messageType);

            var commandId = typedMessage.Metadata.CommandId;

            Clients.All.shipmentCreated(typedMessage.Id, typedMessage.Address);
        }

        public override Task OnConnected()
        {
            if (ConnectedUsers == null)
                ConnectedUsers = new List<string>();

            ConnectedUsers.Add(Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            ConnectedUsers?.Remove(Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}
