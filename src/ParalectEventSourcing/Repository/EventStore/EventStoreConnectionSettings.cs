// <copyright file="EventStoreConnectionSettings.cs" company="Advanced Metering Services LLC">
//     Copyright (c) Advanced Metering Services LLC. All rights reserved.
// </copyright>

namespace ParalectEventSourcing.Repository.EventStore
{
    /// <summary>
    /// GetEventStore connection settings
    /// </summary>
    public class EventStoreConnectionSettings
    {
        /// <summary>
        /// Gets or sets host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets login for connection
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets pass for connection
        /// </summary>
        public string Pass { get; set; }
    }
}