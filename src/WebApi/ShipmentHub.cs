namespace WebApi
{
    using System;
    using System.Threading.Tasks;
    using Contracts.Events;
    using Microsoft.AspNetCore.SignalR;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using ParalectEventSourcing.Serialization;
    using RabbitMQ.Client.Events;

    public class ShipmentHub : Hub
    {
        private readonly CommandConnectionsDictionary _commandConnections;
        private readonly IMessageSerializer _messageSerializer;

        public ShipmentHub(IChannel successChannel, IChannel errorChannel, CommandConnectionsDictionary commandConnections, IMessageSerializer messageSerializer)
        {
            _commandConnections = commandConnections;
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

            var commandId = @event.Metadata.CommandId;
            var eventType = Type.GetType(@event.Metadata.TypeName);

            var connectionId = (string) _commandConnections.GetAndRemoveCommandConnection(commandId); // cast is necessary for call Client(connectionId)

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

            var commandId = (string) message.OriginalCommand.Metadata.CommandId;
            var connectionId = _commandConnections.GetAndRemoveCommandConnection(commandId);

            Clients.Client(connectionId).showErrorMessage(message.ErrorMessage);
        }
    }
}
