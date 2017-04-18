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
        private readonly IMessageSerializer _messageSerializer;
        private readonly IChannel _successChannel;
        private readonly IChannel _errorChannel;

        public ShipmentHub(IChannel successChannel, IChannel errorChannel, IMessageSerializer messageSerializer)
        {
            _messageSerializer = messageSerializer;
            _successChannel = successChannel;
            _errorChannel = errorChannel;
        }

        public void Listen(string connectionToken)
        {
            Task.Run(() =>
            {
                _successChannel.Listen(ExchangeConfiguration.SuccessExchange, ConsumerOnSuccess, connectionToken);
            });

            Task.Run(() =>
            {
                _errorChannel.Listen(ExchangeConfiguration.ErrorExchange, ConsumerOnError, connectionToken);
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
