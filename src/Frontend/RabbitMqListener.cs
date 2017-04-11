namespace Frontend
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.AspNetCore.SignalR.Infrastructure;
    using Newtonsoft.Json;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using RabbitMQ.Client.Events;

    public class RabbitMqListener
    {
        private readonly IHubContext _hubContext;

        public RabbitMqListener(IChannel channel, IConnectionManager connectionManager)
        {
            _hubContext = connectionManager.GetHubContext<ShipmentHub>();

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

            _hubContext.Clients.Clients(ShipmentHub.ConnectedUsers).shipmentCreated(); // TODO not working
        }
    }
}
