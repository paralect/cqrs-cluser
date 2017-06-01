namespace ParalectEventSourcing.Serialization
{
    using System;

    public interface ISerializer
    {
        byte[] Serialize(object data);

        dynamic Deserialize(byte[] objectBinary, Func<dynamic, string> getTypeName);

        dynamic Deserialize(byte[] objectBinary);

        dynamic Deserialize(byte[] objectBinary, Type objectType);
    }
}