namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using System.Collections.Generic;
    using Events;

    public class RabbitMqEventBus : IEventBus
    {
        private readonly IChannel _channel;

        public RabbitMqEventBus(IChannel channel)
        {
            _channel = channel;
        }

        public void Publish(IEvent eventMessage)
        {
            _channel.Send(QueueConfiguration.ReadModelQueue, eventMessage);
        }

        public void Publish(IEnumerable<IEvent> eventMessages)
        {
            foreach (var evnt in eventMessages)
            {
                Publish(evnt);
            }
        }
    }
}
