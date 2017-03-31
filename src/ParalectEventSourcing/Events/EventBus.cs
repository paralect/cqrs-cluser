// <copyright file="EventBus.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Events
{
    using System.Collections.Generic;
    using Dispatching;

    /// <summary>
    /// Events bus which directly pass event to the dispatcher
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly IEventDispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBus"/> class.
        /// </summary>
        /// <param name="dispatcher">the dispatcher</param>
        public EventBus(IEventDispatcher dispatcher)
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