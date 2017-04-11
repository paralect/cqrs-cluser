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
            _channel.QueueDeclare(QueueConfiguration.ReadModelQueue, true, false, false);
            _channel.QueueDeclare(QueueConfiguration.WriteModelQueue, true, false, false);
            _channel.QueueDeclare(QueueConfiguration.ErrorQueue, true, false, false);
            _channel.QueueDeclare(QueueConfiguration.SuccessQueue, true, false, false);

            _messageSerializer = messageSerializer;
        }

        public void Send(string queue, object message)
        {
            _channel.BasicPublish("", queue, null, _messageSerializer.Serialize(message));
        }

        public void Listen(string queue, EventHandler<BasicDeliverEventArgs> callback)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += callback;
            _channel.BasicConsume(queue, true, consumer);

            Console.Read();
        }
    }
}
