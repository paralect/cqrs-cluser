namespace ParalectEventSourcing.Messaging
{
    using System.Text;
    using Commands;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using Utils;
    using WriteModel.Infrastructure.Messaging;

    public class RabbitMqCommandBus : CommandBus
    {
        private const string WriteModelQueue = "WriteModelQueue";
        private readonly IChannelFactory _channelFactory;

        public RabbitMqCommandBus(IDateTimeProvider dateTimeProvider, IChannelFactory channelFactory) 
            : base(dateTimeProvider)
        {
            _channelFactory = channelFactory;
        }

        protected override void SendInternal(params ICommand[] commands)
        {
            using (var channel = _channelFactory.CreateChannel())
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
