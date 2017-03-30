// <copyright file="EventsStream.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Repository.EventStore
{
    using System.Collections.Generic;
    using Events;

    /// <summary>
    /// The stream of events from event store
    /// </summary>
    public class EventsStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventsStream"/> class.
        /// </summary>
        /// <param name="streamId">stream ID</param>
        /// <param name="start">start version</param>
        /// <param name="end">end version</param>
        /// <param name="events">the events</param>
        public EventsStream(string streamId, int start, int end, IEnumerable<IEvent> events)
        {
            this.StreamId = streamId;
            this.Start = start;
            this.End = end;
            this.Events = events;
        }

        /// <summary>
        /// Gets stream ID
        /// </summary>
        public string StreamId { get; private set; }

        /// <summary>
        /// Gets start version
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Gets end version
        /// </summary>
        public int End { get; private set; }

        /// <summary>
        /// Gets event from teh stream
        /// </summary>
        public IEnumerable<IEvent> Events { get; private set; }
    }
}