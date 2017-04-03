namespace WriteModel.Infrastructure.Messaging
{
    using RabbitMQ.Client;

    public class ChannelFactory : IChannelFactory
    {
        private readonly IConnection _connection;

        public ChannelFactory(IConnection connection)
        {
            _connection = connection;
        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }
    }
}
