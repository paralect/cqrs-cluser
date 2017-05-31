namespace ParalectEventSourcing.Serialization
{
    using System;
    using System.Text;
    using Newtonsoft.Json;

    public class DefaultMessageSerializer : IMessageSerializer
    {
        public byte[] Serialize(object message)
        {
            var data = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(data);

            return body;
        }

        public dynamic Deserialize(byte[] objectBinary, Func<dynamic, string> getTypeName)
        {
            var objectString = Encoding.UTF8.GetString(objectBinary);
            var @object = JsonConvert.DeserializeObject(objectString);

            var objectTypeName = getTypeName(@object);
            var objectType = Type.GetType(objectTypeName);
            var stronlyTypedObject = JsonConvert.DeserializeObject(objectString, objectType);

            return stronlyTypedObject;
        }

        public dynamic Deserialize(byte[] objectBinary)
        {
            var objectString = Encoding.UTF8.GetString(objectBinary);
            var @object = JsonConvert.DeserializeObject(objectString);

            return @object;
        }

        public dynamic Deserialize(byte[] objectBinary, Type objectType)
        {
            var objectString = Encoding.UTF8.GetString(objectBinary);
            var @object = JsonConvert.DeserializeObject(objectString, objectType);

            return @object;
        }
    }
}
