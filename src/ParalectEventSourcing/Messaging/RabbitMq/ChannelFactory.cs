namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using System.Threading;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Exceptions;

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

            // TODO WriteModel app crashes on startup, because RabbitMQ container is not ready. Need to find a workaround to this
            while (true)
            {
                try
                {
                    _connection = connectionFactory.CreateConnection();
                    break;
                }
                catch (BrokerUnreachableException) // wait and try again
                {
                    Thread.Sleep(1000);
                }
            }
           
        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }
    }
}
