namespace WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts.Events;
    using Microsoft.AspNetCore.SignalR;
    using Newtonsoft.Json;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using RabbitMQ.Client.Events;

    public class ShipmentHub : Hub
    {
        public static List<string> ConnectedUsers;
        private readonly CommandConnectionsDictionary _commandConnections;

        public ShipmentHub(IChannel successChannel, IChannel errorChannel, CommandConnectionsDictionary commandConnections)
        {
            _commandConnections = commandConnections;

            Task.Run(() =>
            {
                successChannel.Listen(QueueConfiguration.SuccessQueue, ConsumerOnSuccess);
            });

            Task.Run(() =>
            {
                errorChannel.Listen(QueueConfiguration.ErrorQueue, ConsumerOnError);
            });
        }

        private void ConsumerOnSuccess(object sender, BasicDeliverEventArgs e)
        {
            var body = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine("Received: " + body);
            dynamic message = JsonConvert.DeserializeObject(body);

            var typeName = message.Metadata.TypeName.ToString();
            var messageType = Type.GetType(typeName);
            var typedMessage = JsonConvert.DeserializeObject(body, messageType);

            var commandId = typedMessage.Metadata.CommandId;
            var connectionId = (string) _commandConnections.GetAndRemoveCommandConnection(commandId); // cast is necessary for call Client(connectionId)

            // TODO dispatch events
            if (messageType == typeof(ShipmentCreated))
            {
                Clients.Client(connectionId).shipmentCreated(typedMessage.Id, typedMessage.Address);
            }
            else if (messageType == typeof(ShipmentAddressChanged))
            {
                Clients.Client(connectionId).shipmentAddressChanged(typedMessage.Id, typedMessage.NewAddress);
            }
        }

        private void ConsumerOnError(object sender, BasicDeliverEventArgs e)
        {
            var body = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine("Received: " + body);
            dynamic message = JsonConvert.DeserializeObject(body);

            var commandId = (string) message.OriginalCommand.Metadata.CommandId;
            var connectionId = _commandConnections.GetAndRemoveCommandConnection(commandId);

            Clients.Client(connectionId).showErrorMessage(message.ErrorMessage);
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
