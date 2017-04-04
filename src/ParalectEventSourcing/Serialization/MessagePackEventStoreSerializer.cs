namespace ParalectEventSourcing.Serialization
{
    using System;
    using System.Collections.Generic;
    using EventStore.ClientAPI;
    using MsgPack.Serialization;

    public class MessagePackEventStoreSerializer : IEventStoreSerializer
    {
        private const string EventClrTypeHeader = "EventClrTypeName";
        private readonly SerializationContext _serializationContext = SerializationContext.Default;

        public EventData Serialize(object @event, IDictionary<string, object> headers = null)
        {
            var serializer = _serializationContext.GetSerializer<object>();

            var data = serializer.PackSingleObject(@event);

            var typeName = @event.GetType().AssemblyQualifiedName;

            headers = headers ?? new Dictionary<string, object>();
            var eventHeaders = new Dictionary<string, object>(headers)
            {
                { EventClrTypeHeader, typeName }
            };

            var metadata = serializer.PackSingleObject(eventHeaders);

            return new EventData(Guid.NewGuid(), typeName, true, data, metadata);
        }

        public object Deserialize(ResolvedEvent @event)
        {
            return Deserialize(@event.Event.Metadata, @event.Event.Data);
        }

        public object Deserialize(byte[] metadataBinary, byte[] data)
        {
            var metadata = _serializationContext
                .GetSerializer<Dictionary<string, string>>()
                .UnpackSingleObject(metadataBinary);

            var eventType = metadata[EventClrTypeHeader];

            return _serializationContext
                .GetSerializer(Type.GetType(eventType))
                .UnpackSingleObject(data);
        }
    }
}
