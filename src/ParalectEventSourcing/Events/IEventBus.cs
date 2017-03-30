// <copyright file="IEventBus.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Events
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for event bus which incapsulate event messaging transport
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publishes an event to read model.
        /// </summary>
        /// <param name="eventMessage">published event</param>
        void Publish(
            IEvent eventMessage);

        /// <summary>
        /// Publishes events to read model.
        /// </summary>
        /// <param name="eventMessages">events to publish</param>
        void Publish(
            IEnumerable<IEvent> eventMessages);
    }
}