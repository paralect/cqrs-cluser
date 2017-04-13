namespace ParalectEventSourcing.Serialization
{
    using System;

    public interface IMessageSerializer
    {
        byte[] Serialize(object message);

        dynamic Deserialize(byte[] objectBinary, Func<dynamic, string> getTypeName);

        dynamic Deserialize(byte[] objectBinary);
    }
}