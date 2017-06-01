namespace ParalectEventSourcing.Serialization
{
    using System;
    using System.Text;
    using Newtonsoft.Json;

    public class DefaultSerializer : ISerializer
    {
        public byte[] Serialize(object obj)
        {
            var objString = JsonConvert.SerializeObject(obj);
            var objBinary = Encoding.UTF8.GetBytes(objString);

            return objBinary;
        }

        public dynamic Deserialize(byte[] objBinary, Func<dynamic, string> getTypeName)
        {
            var objString = Encoding.UTF8.GetString(objBinary);
            var obj = JsonConvert.DeserializeObject(objString);

            var objTypeName = getTypeName(obj);
            var objType = Type.GetType(objTypeName);
            var typedObj = JsonConvert.DeserializeObject(objString, objType);

            return typedObj;
        }

        public dynamic Deserialize(byte[] objBinary)
        {
            var objString = Encoding.UTF8.GetString(objBinary);
            var obj = JsonConvert.DeserializeObject(objString);

            return obj;
        }

        public dynamic Deserialize(byte[] objBinary, Type objType)
        {
            var objString = Encoding.UTF8.GetString(objBinary);
            var obj = JsonConvert.DeserializeObject(objString, objType);

            return obj;
        }
    }
}
