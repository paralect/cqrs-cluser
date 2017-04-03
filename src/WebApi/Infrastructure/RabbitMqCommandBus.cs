namespace WebApi.Infrastructure
{
    using System.Text;
    using Newtonsoft.Json;
    using ParalectEventSourcing.Commands;
    using ParalectEventSourcing.Utils;
    using RabbitMQ.Client;

    public class RabbitMqCommandBus : CommandBus
    {
        private const string HostName = "localhost";
        private const string WriteModelQueue = "WriteModelQueue";

        public RabbitMqCommandBus(IDateTimeProvider dateTimeProvider) 
            : base(dateTimeProvider)
        {
        }

        protected override void SendInternal(params ICommand[] commands)
        {
            var factory = new ConnectionFactory { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: WriteModelQueue,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                foreach (var command in commands)
                {
                    var data = JsonConvert.SerializeObject(command);
                    var body = Encoding.UTF8.GetBytes(data);

                    channel.BasicPublish(exchange: "",
                                     routingKey: WriteModelQueue,
                                     basicProperties: null,
                                     body: body);
                }
            }
        }
    }
}
