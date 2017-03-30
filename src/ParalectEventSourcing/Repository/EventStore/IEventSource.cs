// <copyright file="IEventSource.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Repository.EventStore
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Events;
    using Exceptions;

    /// <summary>
    /// Inreface of event store
    /// </summary>
    public interface IEventSource
    {
        /// <summary>
        /// Appends event to the event stream
        /// </summary>
        /// <param name="streamId">stream ID</param>
        /// <param name="version">version</param>
        /// <param name="events">events to append</param>
        /// <returns>async void task</returns>
        /// <exception cref="DuplicateTransitionException">When concurrency exception</exception>
        Task AppendEventsAsync(string streamId, int version, IEnumerable<IEvent> events);

        /// <summary>
        /// Gets events stream
        /// </summary>
        /// <param name="streamId">stream ID</param>
        /// <returns>The stream of events</returns>
        EventsStream GetEventsStream(string streamId);

        /// <summary>
        /// Gets events stream
        /// </summary>
        /// <param name="streamId">stream ID</param>
        /// <param name="offset">strarting from this version</param>
        /// <returns>The stream of events</returns>
        EventsStream GetEventsStream(string streamId, int offset);
    }
}
