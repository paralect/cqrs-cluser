// <copyright file="IEventMetadata.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Events
{
    using System;

    /// <summary>
    /// Event metadata interface
    /// </summary>
    public interface IEventMetadata
    {
        /// <summary>
        /// Gets or sets unique Id of event
        /// </summary>
        string EventId { get; set; }

        /// <summary>
        /// Gets or sets command Id of command that initiate this event
        /// </summary>
        string CommandId { get; set; }

        /// <summary>
        /// Gets or sets user Id of user who initiated this event
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// Gets or sets datetime when event was stored in Event Store.
        /// </summary>
        DateTime StoredDate { get; set; }

        /// <summary>
        /// Gets or sets assembly qualified CLR Type name
        /// </summary>
        string TypeName { get; set; }
    }
}
