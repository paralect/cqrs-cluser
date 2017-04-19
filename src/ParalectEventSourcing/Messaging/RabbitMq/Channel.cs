namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using System;
    using System.Threading;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using Serialization;

    public class Channel : ISuccessChannel, IErrorChannel, IWriteModelChannel, IReadModelChannel
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

        public string Subscribe(string exchange, EventHandler<BasicDeliverEventArgs> callback, string routingKey = "")
        {
            var queueName = _channel.QueueDeclare(exchange + "_" + Guid.NewGuid()).QueueName;
            _channel.QueueBind(queueName, exchange, routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += callback;
            var consumerTag = _channel.BasicConsume(queueName, true, Guid.NewGuid().ToString(), consumer);

            return consumerTag;
        }

        public void Unsubscribe(string consumerTag)
        {
            _channel.BasicCancel(consumerTag);
        }
    }
}
