namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using System;
    using RabbitMQ.Client;
    using Serialization;

    public class ChannelFactory : IChannelFactory
    {
        private readonly IConnection _connection;
        private readonly IMessageSerializer _messageSerializer;

        public ChannelFactory(RabbitMqConnectionSettings connectionSettings, IMessageSerializer messageSerializer)
        {
            var connectionFactory = new ConnectionFactory
            {
                UserName = connectionSettings.UserName,
                Password = connectionSettings.Password,
                VirtualHost = connectionSettings.VirtualHost,
                HostName = connectionSettings.HostName,
                Port = connectionSettings.Port,

                AutomaticRecoveryEnabled = true
            };

            _connection = connectionFactory.CreateConnection();

            _connection.ConnectionShutdown += (sender, args) =>
            {
                Console.WriteLine("Connection closed");
                Console.WriteLine("Cause: " + args.Cause);
                Console.WriteLine("Initiator: " + args.Initiator);
                Console.WriteLine("ReplyCode: " + args.ReplyCode);
                Console.WriteLine("ReplyText: " + args.ReplyText);
            };

            _messageSerializer = messageSerializer;
        }

        public Channel CreateChannel()
        {
            var model = _connection.CreateModel();
            return new Channel(model, _messageSerializer);
        }
    }
}
