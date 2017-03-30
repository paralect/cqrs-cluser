// <copyright file="IEvent.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Events
{
    /// <summary>
    /// The event interface
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets aggregate root ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets or sets aggregate root version
        /// </summary>
        int Version { get; set; }

        /// <summary>
        /// Gets or sets event metadata
        /// </summary>
        EventMetadata Metadata { get; set; }
    }
}
