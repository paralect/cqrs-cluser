// <copyright file="DispatcherEventBus.cs" company="Advanced Metering Services LLC">
// Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Events
{
    using System.Collections.Generic;
    using Dispatching;

    /// <summary>
    /// Events bus which directly pass event to the dispatcher
    /// </summary>
    public class DispatcherEventBus : IEventBus
    {
        private readonly IDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherEventBus"/> class.
        /// </summary>
        /// <param name="dispatcher">the dispatcher</param>
        public DispatcherEventBus(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        /// <inheritdoc/>
        public void Publish(
            IEvent eventMessage)
        {
            this.dispatcher.Dispatch(
                eventMessage);
        }

        /// <inheritdoc/>
        public void Publish(
            IEnumerable<IEvent> eventMessages)
        {
            foreach (var evnt in eventMessages)
            {
                this.dispatcher.Dispatch(
                    evnt);
            }
        }
    }
}