namespace WriteModel.Infrastructure
{
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;
    using ParalectEventSourcing.Events;
    using RabbitMQ.Client;

    public class RabbitMqEventBus : IEventBus
    {
        private const string HostName = "localhost";
        private const string ReadModelQueue = "ReadModelQueue";

        public void Publish(IEvent eventMessage)
        {
            var factory = new ConnectionFactory { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
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
            var factory = new ConnectionFactory { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: ReadModelQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                foreach (var evnt in eventMessages)
                {
                    var data = JsonConvert.SerializeObject(evnt);
                    var body = Encoding.UTF8.GetBytes(data);

                    channel.BasicPublish(exchange: "",
                                     routingKey: ReadModelQueue,
                                     basicProperties: null,
                                     body: body);
                }
            }
        }
    }
}
