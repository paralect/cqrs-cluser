// <copyright file="DefaultEventStoreSerializer.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using EventStore.ClientAPI;
    using Newtonsoft.Json;

    /// <summary>
    /// default binary/json serializer
    /// </summary>
    public class DefaultEventStoreSerializer : IEventStoreSerializer
    {
        private const string EventClrTypeHeader = "EventClrTypeName";
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            // ContractResolver = new Platform.Domain.PrivateSetterContractResolver(), TODO make it work
            ContractResolver = new PrivateSetterContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        /// <inheritdoc/>
        public EventData Serialize(object @event, IDictionary<string, object> headers = null)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event, JsonSerializerSettings));

            var typeName = @event.GetType().AssemblyQualifiedName;

            headers = headers ?? new Dictionary<string, object>();
            var eventHeaders = new Dictionary<string, object>(headers)
            {
                { EventClrTypeHeader, typeName }
            };

            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, JsonSerializerSettings));

            return new EventData(Guid.NewGuid(), typeName, true, data, metadata);
        }

        /// <inheritdoc/>
        public object Deserialize(ResolvedEvent @event)
        {
            return Deserialize(@event.Event.Metadata, @event.Event.Data);
        }

        /// <inheritdoc/>
        public object Deserialize(byte[] metadataBinary, byte[] data)
        {
            var metadataJson = Encoding.UTF8.GetString(metadataBinary);
            var metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(metadataJson);
            var eventType = metadata[EventClrTypeHeader];

            var eventDataJson = Encoding.UTF8.GetString(data);

            return JsonConvert.DeserializeObject(eventDataJson, Type.GetType(eventType), JsonSerializerSettings);
        }
    }
}