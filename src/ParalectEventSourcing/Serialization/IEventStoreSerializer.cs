// <copyright file="IEventStoreSerializer.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Serialization
{
    using System.Collections.Generic;
    using EventStore.ClientAPI;

    /// <summary>
    /// The event serializaer interface
    /// </summary>
    public interface IEventStoreSerializer
    {
        /// <summary>
        /// Deserialize event from event store object
        /// </summary>
        /// <param name="event">event store object</param>
        /// <returns>the event</returns>
        object Deserialize(ResolvedEvent @event);

        /// <summary>
        /// Serialize event to event data
        /// </summary>
        /// <param name="event">the events</param>
        /// <param name="headers">the headers</param>
        /// <returns>event data (event + metadata)</returns>
        EventData Serialize(object @event, IDictionary<string, object> headers = null);

        /// <summary>
        /// Deserialize event from binary event metadata and binary event data
        /// </summary>
        /// <param name="metadata">binary event metadata</param>
        /// <param name="data">binary event data</param>
        /// <returns>the event</returns>
        object Deserialize(byte[] metadata, byte[] data);
    }
}
