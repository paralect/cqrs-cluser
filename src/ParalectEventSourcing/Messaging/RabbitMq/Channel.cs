namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using System;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using Serialization;

    public class Channel : IChannel
    {
        private readonly IModel _channel;
        private readonly IMessageSerializer _messageSerializer;

        public Channel(IChannelFactory channelFactory, IMessageSerializer messageSerializer)
        {
            _channel = channelFactory.CreateChannel();

            _channel.ExchangeDeclare(ExchangeConfiguration.WriteModelExchange, "fanout", true);
            _channel.ExchangeDeclare(ExchangeConfiguration.ReadModelExchange, "fanout", true);
            _channel.ExchangeDeclare(ExchangeConfiguration.SuccessExchange, "direct", true);
            _channel.ExchangeDeclare(ExchangeConfiguration.ErrorExchange, "direct", true);

            _messageSerializer = messageSerializer;
        }

        public void Send(string exchange, object message, string routingKey = "")
        {
            _channel.BasicPublish(exchange, routingKey, null, _messageSerializer.Serialize(message));
        }

        public void Listen(string exchange, EventHandler<BasicDeliverEventArgs> callback, string routingKey = "")
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, exchange, routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += callback;
            _channel.BasicConsume(queueName, true, consumer);

            Console.Read();
        }
    }
}
