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

        public string ClusterDns { get; set; }

        public int GossipPort { get; set; }

        public EventStoreConnectionSettings()
        {
#if DEBUG
            Host = "eventstore";
            Port = 1113;
            Login = "admin";
            Pass = "changeit";
#else
            ClusterDns = "eventstore.default.svc.cluster.local";
            GossipPort = 2113;
            Login = "admin";
            Pass = "changeit";
#endif
        }
    }
}