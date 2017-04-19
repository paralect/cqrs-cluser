namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using System.Collections.Generic;
    using Events;

    public class RabbitMqEventBus : IEventBus
    {
        private readonly IReadModelChannel _channel;

        public RabbitMqEventBus(IReadModelChannel channel)
        {
            _channel = channel;
        }

        public void Publish(IEvent eventMessage)
        {
            _channel.Send(ExchangeConfiguration.ReadModelExchange, eventMessage);
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
