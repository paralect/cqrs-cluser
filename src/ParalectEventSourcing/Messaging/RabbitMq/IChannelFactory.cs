namespace ParalectEventSourcing.Messaging.RabbitMq
{
    using RabbitMQ.Client;

    public interface IChannelFactory
    {
        IModel CreateChannel();
    }
}