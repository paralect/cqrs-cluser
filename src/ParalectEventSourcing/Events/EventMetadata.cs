// <copyright file="EventMetadata.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Events
{
    using System;

    /// <summary>
    /// Metadata of particular event
    /// </summary>
    [Serializable]
    public class EventMetadata : IEventMetadata
    {
        /// <summary>
        /// Gets or sets unique Id of event
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Gets or sets command Id of command that initiate this event
        /// </summary>
        public string CommandId { get; set; }

        /// <summary>
        /// Gets or sets user Id of user who initiated this event
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets datetime when event was stored in Event Store.
        /// </summary>
        public DateTime StoredDate { get; set; }

        /// <summary>
        /// Gets or sets assembly qualified Event Type name
        /// </summary>
        public string TypeName { get; set; }
    }
}