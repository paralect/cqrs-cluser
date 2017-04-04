namespace ParalectEventSourcing.Messaging
{
    using System;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class Channel : IChannel
    {
        private readonly IModel _channel;
        private readonly IMessageSerializer _messageSerializer;

        public Channel(IModel channel, IMessageSerializer messageSerializer)
        {
            _channel = channel;
            _messageSerializer = messageSerializer;

            channel.QueueDeclare(QueueConfiguration.ReadModelQueue, true, false, false);
            channel.QueueDeclare(QueueConfiguration.WriteModelQueue, true, false, false);
            channel.QueueDeclare(QueueConfiguration.ErrorQueue, true, false, false);
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
