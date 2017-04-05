// <copyright file="AggregateRoot.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing
{
    using System.Collections.Generic;
    using Events;
    using Exceptions;
    using Utils;

    /// <summary>
    /// Aggregate root base class
    /// </summary>
    public abstract class AggregateRoot : IAggregateRoot
    {
        private readonly List<IEvent> _changes = new List<IEvent>();

        private object _state;

        /// <summary>
        /// Gets recovered aggregate state
        /// </summary>
        public object State => _state;

        /// <summary>
        /// Gets current uncomitted events produced by currently processing command
        /// </summary>
        public IList<IEvent> UncommittedEvents => _changes;

        /// <summary>
        /// Gets versions of the aggreagte
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Sets state and version of the aggregate
        /// </summary>
        /// <param name="state">the state</param>
        /// <param name="version">the verion</param>
        public virtual void Setup(object state, int version)
        {
            Version = version;
            _state = state;
        }

        /// <summary>
        /// Apply event to the state and add to uncommited events
        /// </summary>
        /// <param name="evnt">the event</param>
        public void Apply(IEvent evnt)
        {
            if (string.IsNullOrEmpty(evnt.Id))
            {
                throw new NoAggregateIdException(string.Format("Event {0} has null (or empty) ID property. You may trying to update the aggregate that has not been created. Make sure you specify it on event creation", evnt.GetType().FullName));
            }

            StateSpooler.Spool(_state, evnt);
            _changes.Add(evnt);
        }
    }
}
