namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;
    using Serialization;

    public class ChannelFactory : IChannelFactory
    {
        private readonly IConnection _connection;
        private readonly IMessageSerializer _messageSerializer;

        public ChannelFactory(IOptions<RabbitMqConnectionSettings> connectionSettingsAccessor, IMessageSerializer messageSerializer)
        {
            var connectionSettings = connectionSettingsAccessor.Value;
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

            _messageSerializer = messageSerializer;
        }

        public Channel CreateChannel()
        {
            var model = _connection.CreateModel();
            return new Channel(model, _messageSerializer);
        }
    }
}
