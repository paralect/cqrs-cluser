// <copyright file="IAggregateRoot.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing
{
    using System.Collections.Generic;
    using Events;

    /// <summary>
    /// The aggregate root interface
    /// </summary>
    public interface IAggregateRoot
    {
        /// <summary>
        /// Gets current uncomitted events produced by currently processing command
        /// </summary>
        IList<IEvent> UncommittedEvents { get; }

        /// <summary>
        /// Gets versions of the aggreagte
        /// </summary>
        int Version { get; }

        /// <summary>
        /// Gets aggregate state
        /// </summary>
        object State { get; }
    }
}
