// <copyright file="InMemoryEventBus.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.InMemory
{
    using System.Collections.Generic;
    using Dispatching;
    using Events;

    /// <summary>
    /// Events bus which directly pass event to the dispatcher
    /// </summary>
    public class InMemoryEventBus : IEventBus
    {
        private readonly IDispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryEventBus"/> class.
        /// </summary>
        /// <param name="dispatcher">the dispatcher</param>
        public InMemoryEventBus(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <inheritdoc/>
        public void Publish(
            IEvent eventMessage)
        {
            _dispatcher.Dispatch(
                eventMessage);
        }

        /// <inheritdoc/>
        public void Publish(
            IEnumerable<IEvent> eventMessages)
        {
            foreach (var evnt in eventMessages)
            {
                _dispatcher.Dispatch(
                    evnt);
            }
        }
    }
}