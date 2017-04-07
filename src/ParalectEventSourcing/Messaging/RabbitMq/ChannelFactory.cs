namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using RabbitMQ.Client;

    public class ChannelFactory : IChannelFactory
    {
        private readonly IConnection _connection;

        public ChannelFactory(RabbitMqConnectionSettings connectionSettings)
        {
            var connectionFactory = new ConnectionFactory
            {
                UserName = connectionSettings.UserName,
                Password = connectionSettings.Password,
                VirtualHost = connectionSettings.VirtualHost,
                HostName = connectionSettings.HostName,
                Port = connectionSettings.Port
            };

            _connection = connectionFactory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }
    }
}
