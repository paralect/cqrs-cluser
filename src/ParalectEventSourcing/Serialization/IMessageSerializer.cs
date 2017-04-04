namespace ParalectEventSourcing.Serialization
{
    public interface IMessageSerializer
    {
        byte[] Serialize(object message);
    }
}