namespace ParalectEventSourcing.Serialization
{
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
    }
}
