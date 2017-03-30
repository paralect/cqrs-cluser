// <copyright file="Event.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Events
{
    using System;

    /// <summary>
    /// The event base class
    /// </summary>
    [Serializable]
    public abstract class Event : IEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        protected Event()
        {
            this.Metadata = new EventMetadata();
        }

        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public int Version { get; set; }

        /// <inheritdoc/>
        public EventMetadata Metadata { get; set; }
    }
}