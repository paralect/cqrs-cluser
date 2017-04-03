namespace WriteModel.Infrastructure.Messaging
{
    using RabbitMQ.Client;

    public interface IChannelFactory
    {
        IModel CreateChannel();
    }
}