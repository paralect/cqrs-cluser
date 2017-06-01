namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using Microsoft.Extensions.Options;
    using RabbitMQ.Client;
    using Serialization;

    public class ChannelFactory : IChannelFactory
    {
        private readonly IConnection _connection;
        private readonly ISerializer _serializer;

        public ChannelFactory(IOptions<RabbitMqConnectionSettings> connectionSettingsAccessor, ISerializer serializer)
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

            _serializer = serializer;
        }

        public Channel CreateChannel()
        {
            var model = _connection.CreateModel();
            return new Channel(model, _serializer);
        }
    }
}
