namespace ParalectEventSourcing.Messaging
{
    using System.Collections.Generic;
    using System.Text;
    using Events;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using WriteModel.Infrastructure.Messaging;

    public class RabbitMqEventBus : IEventBus
    {
        private const string ReadModelQueue = "ReadModelQueue";
        private readonly IChannelFactory _channelFactory;

        public RabbitMqEventBus(IChannelFactory channelFactory)
        {
            _channelFactory = channelFactory;
        }

        public void Publish(IEvent eventMessage)
        {
            using (var channel = _channelFactory.CreateChannel())
            {
                channel.QueueDeclare(queue: ReadModelQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var data = JsonConvert.SerializeObject(eventMessage);
                var body = Encoding.UTF8.GetBytes(data);

                channel.BasicPublish(exchange: "",
                                 routingKey: ReadModelQueue,
                                 basicProperties: null,
                                 body: body);
            }
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
