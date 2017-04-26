namespace ParalectEventSourcing.Messaging.RabbitMq
{
    public interface IChannelFactory
    {
        Channel CreateChannel();
    }
}