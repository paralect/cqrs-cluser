// <copyright file="EventSource.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Repository.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Events;
    using Exceptions;
    using global::EventStore.ClientAPI;
    using global::EventStore.ClientAPI.Exceptions;
    using global::EventStore.ClientAPI.SystemData;
    using Serialization;
    using Utils;

    /// <summary>
    /// Event source
    /// </summary>
    public class EventSource : IEventSource
    {
        private readonly IEventStoreConnection _connection;
        private readonly IEventStoreSerializer _serializer;
        private readonly UserCredentials _credentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSource"/> class.
        /// </summary>
        /// <param name="connectionSettings">EventStore connections string</param>
        /// <param name="serializer">events serializator</param>
        public EventSource(EventStoreConnectionSettings connectionSettings, IEventStoreSerializer serializer)
        {
            _serializer = serializer;

            IPEndPoint point = IpEndPointUtility.CreateIpEndPoint(connectionSettings.Host + ":" + connectionSettings.Port).Result;
            _connection = EventStoreConnection.Create(
                ConnectionSettings.Create().KeepReconnecting(), point);
            _connection.ConnectAsync().Wait();
            _credentials = new UserCredentials(connectionSettings.Login, connectionSettings.Pass);
        }

        /// <summary>
        /// Appends event to the event stream
        /// </summary>
        /// <param name="streamId">stream ID</param>
        /// <param name="version">version</param>
        /// <param name="events">events to append</param>
        /// <returns>async void task</returns>
        /// <exception cref="DuplicateTransitionException">When concurrency exception</exception>
        public async Task AppendEventsAsync(string streamId, int version, IEnumerable<IEvent> events)
        {
            var items = events.Select(x => _serializer.Serialize(x));
            try
            {
                await _connection.AppendToStreamAsync(streamId, version, items, _credentials);
            }
            catch (WrongExpectedVersionException e)
            {
                throw new DuplicateTransitionException(streamId, version, e);
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Any(x => x is WrongExpectedVersionException))
                {
                    throw new DuplicateTransitionException(streamId, version, e);
                }
            }
        }

        /// <summary>
        /// Gets events stream
        /// </summary>
        /// <param name="streamId">stream ID</param>
        /// <param name="start">strarting from this version</param>
        /// <returns>The stream of events</returns>
        public EventsStream GetEventsStream(string streamId, int start)
        {
            var streamEvents = new List<ResolvedEvent>();
            const int step = 500;
            var nextSliceStart = start;
            var lastVersion = 0;
            do
            {
                var currentSlice = _connection.ReadStreamEventsForwardAsync(streamId, nextSliceStart, step, false, _credentials).Result;
                nextSliceStart = currentSlice.NextEventNumber;
                streamEvents.AddRange(currentSlice.Events);
                if (currentSlice.IsEndOfStream)
                {
                    lastVersion = currentSlice.LastEventNumber;
                    break;
                }
            }
            while (true);
            var events = streamEvents.Select(x => (IEvent)_serializer.Deserialize(x));
            return new EventsStream(streamId, start, lastVersion, events);
        }

        /// <summary>
        /// Gets events stream
        /// </summary>
        /// <param name="streamId">stream ID</param>
        /// <returns>The stream of events</returns>
        public EventsStream GetEventsStream(string streamId)
        {
            return GetEventsStream(streamId, StreamPosition.Start);
        }
    }
}