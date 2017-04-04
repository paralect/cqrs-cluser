namespace ParalectEventSourcing.Messaging
{
    public interface IMessageSerializer
    {
        byte[] Serialize(object message);
    }
}