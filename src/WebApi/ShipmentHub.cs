namespace WebApi
{
    using System;
    using System.Threading.Tasks;
    using Contracts.Events;
    using Microsoft.AspNetCore.SignalR;
    using ParalectEventSourcing.Commands;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using ParalectEventSourcing.Serialization;
    using RabbitMQ.Client.Events;

    public class ShipmentHub : Hub
    {
        private readonly IMessageSerializer _messageSerializer;

        public ShipmentHub(IChannel successChannel, IChannel errorChannel, IMessageSerializer messageSerializer)
        {
            _messageSerializer = messageSerializer;

            Task.Run(() =>
            {
                successChannel.Listen(QueueConfiguration.SuccessQueue, ConsumerOnSuccess);
            });

            Task.Run(() =>
            {
                errorChannel.Listen(QueueConfiguration.ErrorQueue, ConsumerOnError);
            });
        }

        private void ConsumerOnSuccess(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var @event = _messageSerializer.Deserialize(basicDeliverEventArgs.Body, e => e.Metadata.TypeName);

            var connectionId = (string) @event.Metadata.ConnectionId;
            var eventType = Type.GetType(@event.Metadata.TypeName);

            // TODO dispatch events
            if (eventType == typeof(ShipmentCreated))
            {
                Clients.Client(connectionId).shipmentCreated(@event.Id, @event.Address);
            }
            else if (eventType == typeof(ShipmentAddressChanged))
            {
                Clients.Client(connectionId).shipmentAddressChanged(@event.Id, @event.NewAddress);
            }
        }

        private void ConsumerOnError(object sender, BasicDeliverEventArgs e)
        {
            var message = _messageSerializer.Deserialize(e.Body);

            var connectionId = (string) message.OriginalCommand.Metadata.ConnectionId;

            Clients.Client(connectionId).showErrorMessage(message.ErrorMessage);
        }
    }
}
