namespace WebApi
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Contracts.Events;
    using Microsoft.AspNetCore.SignalR;
    using ParalectEventSourcing.Messaging.RabbitMq;
    using ParalectEventSourcing.Serialization;
    using RabbitMQ.Client.Events;

    public class ShipmentHub : Hub
    {
        private static readonly ConcurrentDictionary<string, IChannel> SuccessSubscriptions = new ConcurrentDictionary<string, IChannel>();
        private static readonly ConcurrentDictionary<string, IChannel> ErrorSubscriptions = new ConcurrentDictionary<string, IChannel>();

        private readonly IChannelFactory _channelFactory;
        private readonly ISerializer _serializer;

        public ShipmentHub(IChannelFactory channelFactory, ISerializer serializer)
        {
            _channelFactory = channelFactory;
            _serializer = serializer;
        }

        public void Listen(string connectionId)
        {
            Task.Run(() =>
            {
                var successChannel = _channelFactory.CreateChannel();
                successChannel.SubscribeToExchange(RabbitMqRoutingConfiguration.SuccessExchange, connectionId, ConsumerOnSuccess);
                SuccessSubscriptions.TryAdd(connectionId, successChannel);
            });

            Task.Run(() =>
            {
                var errorChannel = _channelFactory.CreateChannel();
                errorChannel.SubscribeToExchange(RabbitMqRoutingConfiguration.ErrorExchange, connectionId, ConsumerOnError);
                ErrorSubscriptions.TryAdd(connectionId, errorChannel);
            });
        }

        private void ConsumerOnSuccess(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            var @event = _serializer.Deserialize(basicDeliverEventArgs.Body, e => e.Metadata.TypeName);

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
            var message = _serializer.Deserialize(e.Body);

            var connectionId = (string) message.OriginalCommand.Metadata.ConnectionId;

            Clients.Client(connectionId).showErrorMessage(message.ErrorMessage);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connectionId = Context.ConnectionId;
            IChannel successChannel;
            SuccessSubscriptions.TryRemove(connectionId, out successChannel);
            IChannel errorChannel;
            ErrorSubscriptions.TryRemove(connectionId, out errorChannel);

            successChannel.Close();
            errorChannel.Close();

            return base.OnDisconnected(stopCalled);
        }
    }
}
